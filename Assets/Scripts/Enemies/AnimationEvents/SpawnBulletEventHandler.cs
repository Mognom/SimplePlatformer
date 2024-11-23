using MognomUtils;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpawnBulletEventHandler : MonoBehaviour {

    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private Transform bullet;

    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnAttackAnimationReady() {
        bullet.Spawn(bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        audioSource.Play();
    }
}
