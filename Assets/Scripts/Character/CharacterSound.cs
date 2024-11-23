using UnityEngine;
using static ICharacterAnimable;


[RequireComponent(typeof(AudioSource))]
public class CharacterSound : MonoBehaviour {
    [SerializeField] private AudioClip landClip;
    [SerializeField] private AudioClip jumpClip;

    private ICharacterAnimable character;
    private AudioSource audioSource;

    private void Awake() {
        character = GetComponent<ICharacterAnimable>();
        audioSource = GetComponent<AudioSource>();
        character.OnCurrentActionChange += OnCurrentActionChange;
    }

    private void OnCurrentActionChange(CharacterMovementAction action) {
        switch (action) {
            case CharacterMovementAction.Jump:
                audioSource.clip = jumpClip;
                audioSource.Play();
                break;
            case CharacterMovementAction.Landed:
                //audioSource.clip = landClip;
                //audioSource.Play();
                audioSource.PlayOneShot(landClip);
                break;
            default:
                break;
        }
    }
}
