using UnityEngine;

public class DeathEventHandler : MonoBehaviour {

    private void OnDeathEvent() {
        Destroy(transform.parent.gameObject);
    }
}
