using UnityEngine;

public class EnemyDeathCollider : MonoBehaviour {

    [SerializeField] private int hp = 1;
    [SerializeField] private GameObject parent;

    void OnTriggerEnter2D(Collider2D other) {
        MogPlatformerCharacterController player = other.GetComponent<MogPlatformerCharacterController>();

        if (player != null) {
            player.ForceJump();
            if (--hp <= 0) {
                // TODO death animations
                Destroy(parent);
            }
        }
    }

}
