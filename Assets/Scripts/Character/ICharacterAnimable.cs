using System;

public interface ICharacterAnimable
{
    // Facing direction
    public event Action<float> OnChangeDirection;
    // Movement
    public event Action OnIdleStart;
    public event Action OnWalkStart;
    
    // Jump & Gravity
    public event Action OnJumpStart;
    public event Action OnFallStart;

    // Land event with fall speed
    public event Action<float> OnLanded;

    // Ledge grabbing
    public event Action OnLedgeGrabStart;

    // OneForAll event
    public event Action<CharacterMovementAction> OnCurrentActionChange;

    public enum CharacterMovementAction {
        ChangeDirection,
        Idle,
        Walk,
        Jump,
        Fall,
        Landed,
        Grab
    }
}
