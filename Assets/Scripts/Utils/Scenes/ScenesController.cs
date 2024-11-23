using MognomUtils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Add here all the scenes in the same order than build index
public enum SceneName { None = -1, MainMenu, LevelSelect, Level1, Level2, Level3 }

public class ScenesController : PersistentSingleton<ScenesController> {
    //[Tooltip("Add a loading panel if it need load large scenes")]
    //[SerializeField] private GameObject loadingPanel;

    private SceneName currentScene, lastScene;

    private void Start() {
        currentScene = (SceneName)SceneManager.GetActiveScene().buildIndex;
    }

    public void GoToSceneSync(SceneName name) {
        GoToSceneSync((int)name);
    }

    public void GoToSceneSync(int index) {
        currentScene = (SceneName)index;
        SceneManager.LoadScene(index);
    }

    //--------------unload the actual scene and load a new scene (async)---------------
    public void GoToScene(SceneName name) {
        GoToScene((int)name);
    }

    public void GoToScene(int index) {
        if (index >= 0) {
            StartCoroutine(LoadScene(index));
        }
    }

    IEnumerator LoadScene(int index) {
        //loadingPanel.SetActive(true);
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        lastScene = currentScene;
        currentScene = (SceneName)index;
        while (!op.isDone) {
            yield return null;
        }
        SceneManager.UnloadSceneAsync((int)lastScene);
        //loadingPanel.SetActive(false);
    }


    // Load a new scene without unloading the current one
    public void AddScene(SceneName name) {
        AddScene((int)name);
    }

    public void AddScene(int index) {
        SceneManager.LoadScene(index, LoadSceneMode.Additive);
    }


    // Unload the scene
    public void RemoveScene(SceneName name) {
        RemoveScene((int)name);
    }

    public void RemoveScene(int index) {
        SceneManager.UnloadSceneAsync(index);
    }

    public void RemoveLastScene() {
        RemoveScene(lastScene);
    }


    //--------------------other tools------------------------------


    //reload the current scene (not async)
    public void ReloadCurrentScene() {
        SceneManager.LoadScene((int)currentScene);
    }

    public void QuitGame() {
        Application.Quit();
    }

}
