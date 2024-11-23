using UnityEngine;
using UnityEngine.UI;

public class ToggleButtonSprite : MonoBehaviour {
    [SerializeField] private Sprite enabledSprite;
    [SerializeField] private Sprite disabledSprite;

    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public void Toggle(bool status) {
        if (status) {
            image.sprite = enabledSprite;
        } else {
            image.sprite = disabledSprite;
        }
    }
}
