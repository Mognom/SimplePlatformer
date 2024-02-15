using UnityEngine;
using static ICharacterAnimable;

public class CharacterAnimation : MonoBehaviour {
    private ICharacterAnimable character;
    private Animator animator;
    private SpriteRenderer spriteRender;

    private void Awake() {
        character = GetComponent<ICharacterAnimable>();
        animator = GetComponentInChildren<Animator>();
        spriteRender = animator.gameObject.GetComponent<SpriteRenderer>();
        character.OnCurrentActionChange += OnCurrentActionChange;
        character.OnChangeDirection += OnChangeDirection;
    }

    private void OnChangeDirection(float direction) {
        spriteRender.flipX = Mathf.Sign(direction) == -1;
    }

    private void OnCurrentActionChange(CharacterMovementAction action) {
        switch (action) {
            case CharacterMovementAction.Walk:
                animator.CrossFade("Knight_Run", 0, 0);
                break;
            case CharacterMovementAction.Idle:
                animator.CrossFade("Knight_Idle", 0, 0);
                break;
            default:
                break;
        }
    }
}
