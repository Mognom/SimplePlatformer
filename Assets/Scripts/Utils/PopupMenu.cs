using UnityEngine;
using UnityEngine.InputSystem;

public class PopupMenu : MonoBehaviour {

    private MenuInputActions menuActions;
    private bool currentStatus = false;

    private Canvas canvas;
    private void Awake() {
        canvas = GetComponent<Canvas>();
        canvas.enabled = currentStatus;

        menuActions = new MenuInputActions();
        menuActions.PopupMenu.Enable();

        menuActions.PopupMenu.ToggleMenu.performed += OnMenuToggle;
    }

    private void OnMenuToggle(InputAction.CallbackContext _) {
        OnMenuToggle();
    }

    private void OnMenuToggle() {
        currentStatus = !currentStatus;
        canvas.enabled = currentStatus;
        // if menu enabled pause game
        Time.timeScale = currentStatus ? 0f : 1f;
    }


    public void OnResumePressed() {
        OnMenuToggle();
    }

    public void OnExitPressed() {

    }

    public void OnMusicMutePressed(ToggleButtonSprite tbs) {
        tbs.Toggle(false);
    }

    public void OnEffectsMutePressed(ToggleButtonSprite tbs) {
        tbs.Toggle(false);
    }
}
