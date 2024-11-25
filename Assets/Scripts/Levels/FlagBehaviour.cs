using System.Collections;
using UnityEngine;

public class FlagBehaviour : MonoBehaviour {

    private AudioSource audioSource;
    private bool done = false;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        MogPlatformerCharacterController player = collision.GetComponent<MogPlatformerCharacterController>();

        if (player != null && !done) {
            done = true;
            StartCoroutine(WaitAndComplete());
        }
    }

    IEnumerator WaitAndComplete() {
        audioSource.Play();
        yield return new WaitForSeconds(.3f);
        LevelController.I.OnLevelComplete();
    }
}
