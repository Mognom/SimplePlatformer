using UnityEngine;

public class FlagBehaviour : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        MogPlatformerCharacterController player = collision.GetComponent<MogPlatformerCharacterController>();

        if (player != null) {
            LevelController.I.OnLevelComplete();
        }
    }
}
