using System;
using System.Collections;
using UnityEngine;
using static ICharacterAnimable;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class MogPlatformerCharacterController : MonoBehaviour, ICharacterAnimable {
    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;
    [SerializeField] private MogMovementControllerStatsSO statsSO;

    // Ground values
    private LayerMask currentCollisionLayer;

    // Gravity & fall values
    private float currentFallVelocity;
    private bool isFalling = false;

    // Jump values
    private bool isGrounded;
    private float lastJumpInputTime = -1f;
    private float lastGroundedTime = -1f;

    // Ledges
    private int isGrabbingLedge = 0; // 0= no, -1 = left, 1 = right
    private bool isDropping = false;

    // Input
    private float horizontalMoveValue;
    private bool isSprinting;

    // Animation states
    private CharacterMovementAction currentAction;
    private float lastDirection;
    // Animation events
    public event Action<float> OnChangeDirection;
    public event Action OnIdleStart;
    public event Action OnWalkStart;
    public event Action OnJumpStart;
    public event Action OnFallStart;
    public event Action<float> OnLanded;
    public event Action OnLedgeGrabStart;
    public event Action<CharacterMovementAction> OnCurrentActionChange;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        currentCollisionLayer = statsSO.DefaultColliderMask;
    }

    public void SetHorizontalMoveValue(float horizontalMoveValue) {
        this.horizontalMoveValue = horizontalMoveValue;
        this.isSprinting = false;
    }

    public void SetHorizontalMoveValue(float horizontalMoveValue, bool isSprinting) {
        this.horizontalMoveValue = horizontalMoveValue;
        this.isSprinting = isSprinting;
    }

    private void Update() {
        // Try to grab ledges if the player is falling down and not actively dropping down
        if (isFalling && !isDropping) {
            if (horizontalMoveValue != 0) {
                TryGrabLedge(transform.position, horizontalMoveValue);
            }
        }

        // handle horizontal movement
        Vector2 targetVelocity = Vector2.zero;

        // Check ground
        SetIsGrounded();

        // Attemp to jump
        if (lastJumpInputTime >= Time.time - statsSO.BufferJumpTime) {
            PerformJump();
        }


        // If moving opposite of the ledge direction, let go
        if (horizontalMoveValue + isGrabbingLedge == 0) {
            isGrabbingLedge = 0;
        } else {
            // Movement IS disabled if grabbing a ledge
            if (isGrabbingLedge != 0) {
                rb.velocity = Vector2.zero;
                currentFallVelocity = 0;
                return;
            }
        }

        if (isSprinting) {
            targetVelocity.x = horizontalMoveValue * statsSO.RunSpeed;
        } else {
            targetVelocity.x = horizontalMoveValue * statsSO.MoveSpeed;
        }

        targetVelocity.y = HandleVerticalVelocity();

        if (horizontalMoveValue != 0 && Mathf.Sign(horizontalMoveValue) != lastDirection) {
            lastDirection = Mathf.Sign(horizontalMoveValue);
            ChangeCurrentAction(CharacterMovementAction.ChangeDirection, lastDirection);
        }
        // If not moving vertically nor grabbing ledge, set walk/idle status
        if (isGrounded && isGrabbingLedge == 0) {
            if (horizontalMoveValue != 0) {
                ChangeCurrentAction(CharacterMovementAction.Walk);
            } else {
                ChangeCurrentAction(CharacterMovementAction.Idle);
            }
        }

        // Try to move the players to the desired position
        rb.velocity = targetVelocity;
    }

    // Calculate the frames vertical velocity based on jumping/falling/grounded status
    private float HandleVerticalVelocity() {
        float velocityToApply;
        // handle vertical movement
        if (currentFallVelocity > 0) {
            // Is jumping / going up
            // Stop going up if hit a wall or reached top part of the jump; Start falling back down
            if (IsHittingCeiling()) {
                currentFallVelocity = 0;
                velocityToApply = 0;
            } else {
                // reduce the current vertical speed by applying gravity
                currentFallVelocity = Math.Max(currentFallVelocity + statsSO.JumpGravity * Time.deltaTime, -statsSO.TerminalVelocity);
                velocityToApply = currentFallVelocity;
            }
        } else {
            if (!isGrounded) {
                // Fall down
                currentFallVelocity = Math.Max(currentFallVelocity + statsSO.FallGravity * Time.deltaTime, -statsSO.TerminalVelocity);
                velocityToApply = currentFallVelocity;
                isFalling = true;
                ChangeCurrentAction(CharacterMovementAction.Fall);
            } else {
                // Grounded movement
                if (isFalling) {
                    ChangeCurrentAction(CharacterMovementAction.Landed, currentFallVelocity);

                    currentFallVelocity = 0;
                    isFalling = false;
                }

                // "Hug" ground only while moving
                if (horizontalMoveValue != 0) {
                    velocityToApply = statsSO.HugGroundVelocity;
                } else {
                    velocityToApply = 0;
                }
            }
        }

        return velocityToApply;
    }

    public void OnJumpCanceled() {
        if (currentFallVelocity > 0) {
            // Slow down based on the Cancel multiplier
            currentFallVelocity *= statsSO.CancelVelocityMultiplier;
        }
        lastJumpInputTime = -1;
    }

    public void OnJumpStarted() {
        lastJumpInputTime = Time.time;
    }

    private void PerformJump() {
        if (statsSO.CoyoteJumpTime >= Time.time - lastGroundedTime) {
            lastJumpInputTime = -1;
            if (!isGrounded) {
                Debug.Log("Coyote Jumps!");
            }
            currentFallVelocity = statsSO.InitialJumpSpeed;
            isGrabbingLedge = 0;
            ChangeCurrentAction(CharacterMovementAction.Jump);
        }
    }

    // Enable dropping through platforms, drop ledge if holding one
    public void OnDropPerformed() {
        if (isGrabbingLedge != 0) {
            // Let go of ledge
            isGrabbingLedge = 0;
        }
        isDropping = true;

        currentCollisionLayer = statsSO.GroundColliderMask;
        rb.excludeLayers = statsSO.LedgeColliderMask + statsSO.PlatformColliderMask;
    }

    public void OnDropCanceled() {
        isDropping = false;
        StartCoroutine(ReEnablePlatformCollisionsWithDelay());
    }
    private IEnumerator ReEnablePlatformCollisionsWithDelay() {
        yield return new WaitForSeconds(0.1f);
        currentCollisionLayer = statsSO.DefaultColliderMask;
        rb.excludeLayers = statsSO.LedgeColliderMask;
    }

    /* 
     * Try to grab a ledge in the given direction.
     * If a ledge is found, grab it and stop falling
     */
    private bool TryGrabLedge(Vector3 newPosition, float horizontalDirection) {
        // Can only grab a ledge if falling down
        Vector2 directionVector = new(horizontalDirection, 0);

        Vector2 ledgeCheckLocation = (Vector2)newPosition + new Vector2(horizontalDirection * statsSO.HorizontalHitBoxRadious, statsSO.VerticalHitBoxRadious);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)ledgeCheckLocation, directionVector, statsSO.LedgeGrabDistance, statsSO.LedgeColliderMask);

        if (hit.collider) {
            transform.position = hit.collider.transform.position + new Vector3(-horizontalDirection * statsSO.HorizontalHitBoxRadious, -statsSO.VerticalHitBoxRadious, 0);
            GrabLedge((int)horizontalDirection);
            return true;
        }

        return false;
    }
    private void GrabLedge(int horizontalDirection) {
        isGrabbingLedge = horizontalDirection;
        isFalling = false;
        ChangeCurrentAction(CharacterMovementAction.Grab);
        OnLedgeGrabStart?.Invoke();
    }

    // Check if the object is standing on ground
    private void SetIsGrounded() {
        if (isGrabbingLedge != 0) {
            isGrounded = true;
        } else {
            isGrounded = CheckGroundHits(true);
        }

        if (isGrounded) {
            lastGroundedTime = Time.time;
        }
    }

    private bool IsHittingCeiling() {
        return CheckGroundHits(false);
    }

    private bool CheckGroundHits(bool isDownwards) {
        // Disable hits against already colliding objects
        bool _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        Physics2D.queriesStartInColliders = false;

        Vector2 direction;
        LayerMask layerMask = currentCollisionLayer;
        if (isDownwards) {
            direction = Vector2.down;
        } else {
            // Don't check agaisnt one-way platforms when going up
            direction = Vector2.up;
            layerMask -= statsSO.PlatformColliderMask;
        }

        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(capsuleCollider2D.bounds.center - (Vector3)direction * statsSO.GroundCheckDistance / 2, capsuleCollider2D.size, capsuleCollider2D.direction, 0, direction, statsSO.GroundCheckDistance * 1.5f, layerMask);
        foreach (RaycastHit2D hit in hits) {
            if (hit.collider != null) {
                // Check if the angle is steep enough for it to be consider a valir floor/ceiling
                if (Vector2.Dot(direction, hit.normal) < -statsSO.MinimumDotProductForGround) {
                    Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
                    return true;
                }
            }
        }
        // Restore original values
        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        return false;
    }

    // Change the current animation action and fire its events
    private void ChangeCurrentAction(CharacterMovementAction newAction, float eventValue = 0f) {
        if (currentAction != newAction) {
            currentAction = newAction;
            OnCurrentActionChange?.Invoke(currentAction);
            switch (currentAction) {
                case CharacterMovementAction.ChangeDirection:
                    OnChangeDirection?.Invoke(eventValue); break;
                case CharacterMovementAction.Idle:
                    OnIdleStart?.Invoke(); break;
                case CharacterMovementAction.Walk:
                    OnWalkStart?.Invoke(); break;
                case CharacterMovementAction.Jump:
                    OnJumpStart?.Invoke(); break;
                case CharacterMovementAction.Fall:
                    OnFallStart?.Invoke(); break;
                case CharacterMovementAction.Grab:
                    OnLedgeGrabStart?.Invoke(); break;
                case CharacterMovementAction.Landed:
                    OnLanded?.Invoke(eventValue); break;
                default: break;
            }
        }
    }
}
