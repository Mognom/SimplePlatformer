using UnityEngine;

public class PlayerDeathCollider : MonoBehaviour {

    [SerializeField] private int hp = 1;
    [SerializeField] private GameObject parent;

    void OnTriggerEnter2D(Collider2D other) {
        IEnemyDamageDealer enemy = other.GetComponent<IEnemyDamageDealer>();

        if (enemy != null) {
            if (--hp <= 0) {
                // TODO death animations and reset scene
                //Destroy(parent);
            }
        }
    }

}
