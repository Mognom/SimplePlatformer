using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class EnemyDeathCollider : MonoBehaviour {
    private const string HIT_ANIMATION_NAME = "Hit"; // This animation names are shared through all enemies
    private const string DEATH_ANIMATION_NAME = "Death";
    [SerializeField] private int hp = 1;
    [SerializeField] private GameObject parent;

    private Animator animator;
    private AudioSource audioSource;
    private void Start() {
        animator = parent.GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        MogPlatformerCharacterController player = other.GetComponent<MogPlatformerCharacterController>();

        if (player != null) {
            player.ForceJump(.7f);
            audioSource.Play();
            if (--hp <= 0) {
                animator.CrossFade(DEATH_ANIMATION_NAME, 0, 0);
            } else {
                animator.CrossFade(HIT_ANIMATION_NAME, 0, 0);
            }
        }
    }
}
