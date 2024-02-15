using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    private PlayerInputActions playerInputActions;
    private MogPlatformerCharacterController playerController;

    void Awake() {
        playerController = GetComponent<MogPlatformerCharacterController>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Move.Enable();

        playerInputActions.Move.Jump.started += OnJumpStarted;
        playerInputActions.Move.Jump.canceled += OnJumpCanceled;

        playerInputActions.Move.Drop.performed += OnDropPerformed;
        playerInputActions.Move.Drop.canceled += OnDropCanceled;
    }

    // Update is called once per frame
    void Update() {
        float horizontalMoveInput = playerInputActions.Move.Movement.ReadValue<float>();
        float sprintHeld = playerInputActions.Move.Sprint.ReadValue<float>();
        playerController.SetHorizontalMoveValue(horizontalMoveInput, sprintHeld == 1);
    }
    private void OnJumpCanceled(InputAction.CallbackContext context) {
        playerController.OnJumpCanceled();
    }

    private void OnJumpStarted(InputAction.CallbackContext context) {
        playerController.OnJumpStarted();
    }

    // Enable dropping through platforms, drop ledge if holding one
    private void OnDropPerformed(InputAction.CallbackContext context) {
        playerController.OnDropPerformed();

    }

    private void OnDropCanceled(InputAction.CallbackContext context) {
        playerController.OnDropCanceled();
    }
}
