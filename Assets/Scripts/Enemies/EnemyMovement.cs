using UnityEngine;


public class EnemyMovement : MonoBehaviour, IEnemyDamageDealer {

    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToTurn;

    private float currentTimeToTurn;
    private Rigidbody2D rb;

    private void Awake() {
        currentTimeToTurn = timeToTurn;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = moveSpeed * transform.right;
    }

    private void Update() {
        if ((currentTimeToTurn -= Time.deltaTime) <= 0) {
            currentTimeToTurn = timeToTurn;

            // flip to face towards the new direciton
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, 180, 0);
            //Vector3 scale = transform.localScale;
            //scale.x *= -1;
            //transform.localScale = scale;

            // Update movement
            rb.velocity = moveSpeed * transform.right;
        }
    }
}
