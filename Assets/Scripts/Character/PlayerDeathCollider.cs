using MognomUtils;
using UnityEngine;

public class PlayerDeathCollider : MonoBehaviour {

    [SerializeField] private int hp = 1;
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject deadPlayer;

    void OnTriggerEnter2D(Collider2D other) {
        IEnemyDamageDealer enemy = other.GetComponent<IEnemyDamageDealer>();

        if (enemy != null) {
            if (--hp <= 0) {
                // Replace the player with the death version
                deadPlayer.Spawn(parent.transform.position, Quaternion.identity);
                Destroy(parent);
            }
        }
    }

}
