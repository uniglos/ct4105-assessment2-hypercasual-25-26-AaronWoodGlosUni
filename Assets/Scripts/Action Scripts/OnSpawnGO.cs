using UnityEngine;
using UnityEngine.Events;

public class OnSpawnGO : MonoBehaviour {
    UnityEvent onGameObjectStart;

    private void Start() {
        onGameObjectStart.Invoke();
    }
}