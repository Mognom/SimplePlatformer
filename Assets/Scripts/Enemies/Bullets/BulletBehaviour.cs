using MognomUtils;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour, IEnemyDamageDealer {

    [SerializeField] private float speed;

    private void Update() {
        transform.position += speed * Time.deltaTime * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        this.Recycle();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        this.Recycle();
    }
}
