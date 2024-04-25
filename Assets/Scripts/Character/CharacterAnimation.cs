using UnityEngine;
using static ICharacterAnimable;

public class CharacterAnimation : MonoBehaviour {
    private ICharacterAnimable character;
    [Tooltip("Optionally pass a specifig player animator. Otherwise, it will deafault to the first found in children")]
    [SerializeField] private Animator playerAnimator;
    private SpriteRenderer spriteRender;

    [SerializeField] private string idleAnimationName;
    [SerializeField] private string walkAnimationName;
    [SerializeField] private string jumpAnimationName;
    [SerializeField] private float jumpAnimationDuration;
    [SerializeField] private string landAnimationName;
    [SerializeField] private float landAnimationDuration;
    [SerializeField] private string slideAnimationName;

    [SerializeField] private ParticleSystem moveParticles;
    [SerializeField] private ParticleSystem slideParticles;
    [SerializeField] private ParticleSystem jumpParticles;
    [SerializeField] private ParticleSystem landParticles;

    private int idleAnimation;
    private int walkAnimation;
    private int jumpAnimation;
    private int landAnimation;
    private int slideAnimation;

    private float currentDirection;
    [SerializeField] private float walkMaxTilt = 5;
    [SerializeField] private float walkTiltSpeed = 20;

    private AnimationState currentState;
    private AnimationState nextState;

    private AudioSource aSource;
    private void Start() {
        character = GetComponent<ICharacterAnimable>();
        aSource = GetComponent<AudioSource>();
        if (playerAnimator == null) {
            playerAnimator = GetComponentInChildren<Animator>();
        }
        spriteRender = playerAnimator.gameObject.GetComponent<SpriteRenderer>();
        character.OnCurrentActionChange += OnCurrentActionChange;
        character.OnChangeDirection += OnChangeDirection;

        currentState = new AnimationState();
        currentState.action = CharacterMovementAction.Idle;

        idleAnimation = Animator.StringToHash(idleAnimationName);
        walkAnimation = Animator.StringToHash(walkAnimationName);
        jumpAnimation = Animator.StringToHash(jumpAnimationName);
        landAnimation = Animator.StringToHash(landAnimationName);
        slideAnimation = Animator.StringToHash(slideAnimationName);
    }

    private void Update() {
        UpdateCurrentAnimation();

        HandleCharacterTilt();
    }

    private void UpdateCurrentAnimation() {
        if (nextState.action != currentState.action) {
            if (currentState.lockUntilTime <= Time.time) {
                currentState = nextState;
                nextState.lockUntilTime = 0;
                PlayerCurrentAnimation(currentState);
            }
        }
    }

    private void OnCurrentActionChange(CharacterMovementAction action) {
        if (nextState.lockUntilTime < Time.time) {
            nextState = new AnimationState();

            nextState.action = action;
            switch (action) {
                case CharacterMovementAction.Walk:
                    nextState.animation = walkAnimation;
                    moveParticles.Play();
                    slideParticles.Stop();
                    break;
                case CharacterMovementAction.WallSlide:
                    nextState.animation = slideAnimation;
                    slideParticles.Play();
                    moveParticles.Stop();
                    break;
                case CharacterMovementAction.Jump:
                    nextState.lockUntilTime = jumpAnimationDuration + Time.time;
                    nextState.animation = jumpAnimation;
                    jumpParticles.Play();
                    moveParticles.Stop();
                    slideParticles.Stop();
                    break;
                case CharacterMovementAction.Landed:
                    landParticles.Play();
                    slideParticles.Stop();
                    break;
                case CharacterMovementAction.Idle:
                    nextState.animation = idleAnimation;
                    moveParticles.Stop();
                    slideParticles.Stop();
                    break;
                default:
                    break;
            }
        }

        UpdateCurrentAnimation();
    }

    private void PlayerCurrentAnimation(AnimationState state) {
        playerAnimator.CrossFade(state.animation, 0, 0);
    }
    private void OnChangeDirection(float direction) {
        currentDirection = direction;
        spriteRender.flipX = Mathf.Sign(direction) == -1;
    }

    private void HandleCharacterTilt() {
        Quaternion walkTilt;
        if (currentState.action == CharacterMovementAction.Walk) {
            walkTilt = Quaternion.Euler(0, 0, walkMaxTilt * currentDirection);
        } else {
            walkTilt = Quaternion.identity;
        }
        playerAnimator.transform.up = Vector3.RotateTowards(playerAnimator.transform.up, walkTilt * Vector2.up, walkTiltSpeed * Time.deltaTime, 0f);
    }


    private struct AnimationState {
        public CharacterMovementAction action;
        public float lockUntilTime;
        public int animation;
    }
}
