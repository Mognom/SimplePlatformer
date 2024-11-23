using UnityEngine;

public class ChangeSceneButton : MonoBehaviour {
    [SerializeField] private SceneName targetScene;

    public void OnPressed() {
        ScenesController.I.GoToScene(targetScene);
    }
}
