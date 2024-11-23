using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour {
    private const string ATTACK_ANIMATION_NAME = "Attack";

    [SerializeField] private float attackCooldown;
    private float lastAttackTime;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform bullet;

    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask playerMask;

    private Animator animator;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {

        if (Time.time >= lastAttackTime + attackCooldown) {
            TryAttack();
        }
    }

    private void TryAttack() {
        if (CheckPlayerInAttackRange()) {
            animator.CrossFade(ATTACK_ANIMATION_NAME, 0, 0);
            //bullet.Spawn(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            lastAttackTime = Time.time;
        }
    }

    private bool CheckPlayerInAttackRange() {
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.right, detectionRange, playerMask);
        return hit.collider != null && hit.collider.gameObject.GetComponent<PlayerInput>() != null;
    }
}
