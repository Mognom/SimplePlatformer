
using MognomUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelController : Singleton<LevelController> {
    [SerializeField] private int levelNumber;
    protected override void Awake() {
        base.Awake();
        PlayerInputActions input = new PlayerInputActions();
        input.Reset.Enable();

        input.Reset.Reset.performed += OnResetPressed;
    }

    private void OnResetPressed(InputAction.CallbackContext context) {
        ScenesController.I.ReloadCurrentScene();
    }

    public void OnLevelComplete() {
        int bestLevel = Mathf.Max(levelNumber + 1, PlayerPrefs.GetInt("PlayerPrefsUnlockedLevels", 1));
        PlayerPrefs.SetInt("PlayerPrefsUnlockedLevels", bestLevel);

        // TODO add some delay // fade // win sound
        ScenesController.I.GoToScene(SceneName.LevelSelect);
    }
}
