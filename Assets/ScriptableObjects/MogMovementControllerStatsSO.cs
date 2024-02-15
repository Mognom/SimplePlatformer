using UnityEngine;

[CreateAssetMenu]
public class MogMovementControllerStatsSO : ScriptableObject {
    [Tooltip("Horizontal speed")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float runSpeed = 6;

    [Header("Ground values")]
    [Tooltip("Layer mask that should be used to detect ground collisions")]
    [SerializeField] private LayerMask defaultColliderMask;
    [Tooltip("Layer mask that identifies solid ground, that cannot be passed through")]
    [SerializeField] private LayerMask groundColliderMask;
    [Tooltip("Layer mask used to detect  platforms that the character can pass through")]
    [SerializeField] private LayerMask platformColliderMask;
    [Tooltip("Distance used when checking for ground and ceiling collisions")]
    [SerializeField] private float groundCheckDistance = .05f;
    [Tooltip("Defines the magnitude of the angle. Values from 1 to 0, 1 being only vertical hits, 0 being all hits")]
    [SerializeField] private float minimumDotProductForGround = .8f;

    [Header("Gravity & fall values")]
    [Tooltip("Max vertical speed when falling")]
    [SerializeField] private float terminalVelocity = 16;
    [Tooltip("Gravity applied when going up, acceleration downwards")]
    [SerializeField] private float jumpGravity = -32;
    [Tooltip("Gravity applied when going down, acceleration downwards")]
    [SerializeField] private float fallGravity = -40;
    [Tooltip("Velocity applied dowmwards while grounded, helps with slopes")]
    [SerializeField] private float hugGroundVelocity = -2;

    [Header("Jump values")]
    [Tooltip("Initial velocity upwards when jumping")]
    [SerializeField] private float initialJumpSpeed = 16;
    [Tooltip("Grace time where the player can jump while no longer being grounded, for example, jumping right after falling of the edge of a platform")]
    [SerializeField] private float coyoteJumpTime = .1f;
    [Tooltip("Grace time where the jump input of the player is kept, for example if pressing right before touching the ground/being grounded")]
    [SerializeField] private float bufferJumpTime = .1f;
    [Tooltip("Velocity multiplier when jump is let go. 0 makes the character start falling instantly")]
    [SerializeField] private float cancelVelocityMultiplier = .1f;

    [Header("Ledges")]
    [Tooltip("Layer mask used to detect ledge collisions")]
    [SerializeField] private LayerMask ledgeColliderMask;
    [Tooltip("Distance from which the ledge can be grabbed"), Range(-1, 1)]
    [SerializeField] private float ledgeGrabDistance = .1f;
    [Tooltip("Vertical hitbox radious, used to define the top point from where the character can grab onto ledges")]
    [SerializeField] private float verticalHitBoxRadious = .5f;
    [Tooltip("Horizontal hitbox radious")]
    [SerializeField] private float horizontalHitBoxRadious = .25f;

    [Header("Wall slide")]
    [Tooltip("Max vertical speed when wall sliding")]
    [SerializeField] private float slideTerminalVelocity = 4;

    public float MoveSpeed => moveSpeed;
    public float RunSpeed => runSpeed;
    public LayerMask DefaultColliderMask => defaultColliderMask;
    public LayerMask GroundColliderMask => groundColliderMask;
    public LayerMask PlatformColliderMask => platformColliderMask;
    public float GroundCheckDistance => groundCheckDistance;
    public float MinimumDotProductForGround => minimumDotProductForGround;

    public float TerminalVelocity => terminalVelocity;
    public float JumpGravity => jumpGravity;
    public float FallGravity => fallGravity;
    public float HugGroundVelocity => hugGroundVelocity;

    public float InitialJumpSpeed => initialJumpSpeed;
    public float CoyoteJumpTime => coyoteJumpTime;
    public float BufferJumpTime => bufferJumpTime;
    public float CancelVelocityMultiplier => cancelVelocityMultiplier;

    public LayerMask LedgeColliderMask => ledgeColliderMask;
    public float LedgeGrabDistance => ledgeGrabDistance;
    public float VerticalHitBoxRadious => verticalHitBoxRadious;
    public float HorizontalHitBoxRadious => horizontalHitBoxRadious;

    public float SlideTerminalVelocity => slideTerminalVelocity;
}
