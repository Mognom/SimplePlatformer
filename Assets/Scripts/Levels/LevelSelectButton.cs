using UnityEngine;

public class LevelSelectButton : MonoBehaviour {

    [SerializeField] private float disabledAlpha = .6f;
    [SerializeField] public SceneName targetScene;

    private CanvasGroup canvasGroup;
    private bool isLevelEnabled = false;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();

        // read level from playerPrefs
        int unlockedLevels = PlayerPrefs.GetInt("PlayerPrefsUnlockedLevels", 1);

        if (targetScene <= SceneName.LevelSelect + unlockedLevels) {
            isLevelEnabled = true;
            canvasGroup.alpha = 1;
        } else {
            canvasGroup.alpha = disabledAlpha;
        }
    }

    public void OnClick() {
        if (isLevelEnabled) {
            ScenesController.I.GoToScene(targetScene);
        }
    }
}
