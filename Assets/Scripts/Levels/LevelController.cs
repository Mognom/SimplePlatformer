
using MognomUtils;
using UnityEngine;

public class LevelController : Singleton<LevelController> {
    [SerializeField] private int levelNumber;


    // Reset player position
    private void OnPlayerDeath() {

    }

    public void OnLevelComplete() {
        int bestLevel = Mathf.Max(levelNumber + 1, PlayerPrefs.GetInt("PlayerPrefsUnlockedLevels", 1));
        PlayerPrefs.SetInt("PlayerPrefsUnlockedLevels", bestLevel);

        // TODO add some delay // fade // win sound
        ScenesController.I.GoToScene(SceneName.LevelSelect);
    }
}
