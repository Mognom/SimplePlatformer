using MognomUtils;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour {


    [SerializeField] private float attackCooldown;
    private float lastAttackTime;

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform bullet;

    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask playerMask;


    private void Start() {

    }

    private void Update() {

        if (Time.time >= lastAttackTime + attackCooldown) {
            TryAttack();
        }
    }

    private void TryAttack() {
        if (CheckPlayerInAttackRange()) {
            bullet.Spawn(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }

        // Attemp is wasted even if it didnt shoot
        lastAttackTime = Time.time;
    }

    private bool CheckPlayerInAttackRange() {
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPoint.position, bulletSpawnPoint.right, detectionRange, playerMask);
        return hit.collider != null;
    }
}
