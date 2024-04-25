using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField] private float moveSpeed;
    [SerializeField] private float timeToTurn;

    private float currentTimeToTurn;
    private Rigidbody2D rb;
    private float direction;



    private void Awake() {
        currentTimeToTurn = timeToTurn;
        rb = GetComponent<Rigidbody2D>();
        direction = 1;
        rb.velocity = moveSpeed * direction * Vector3.right;
    }

    private void Update() {
        if ((currentTimeToTurn -= Time.deltaTime) <= 0) {
            currentTimeToTurn = timeToTurn;
            direction *= -1;

            // flip to face towards the new direciton 
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            // Update movement
            rb.velocity = moveSpeed * direction * Vector3.right;
        }
    }
}
