using MognomUtils;
using UnityEngine;

public class SpawnBulletEventHandler : MonoBehaviour {

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform bullet;

    private void OnAttackAnimationReady() {
        bullet.Spawn(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
