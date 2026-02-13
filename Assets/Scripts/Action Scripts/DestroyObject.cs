using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] UnityEvent onDestroy;
    [SerializeField] UnityEvent<GameObject> onDestroyObjectPassSelf;

    public void DestroyObj(GameObject destroyingObject) {
        Destroy(destroyingObject);
    }
    public void DestroySelf() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        onDestroy.Invoke();
        onDestroyObjectPassSelf.Invoke(gameObject);
    }
}
