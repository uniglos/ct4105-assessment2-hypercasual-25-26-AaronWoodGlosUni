using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SetUnityObject : MonoBehaviour
{
    [SerializeField] UnityEvent<object> setObjectEvent;
    [SerializeField] UnityEvent<GameObject> setGameObjectEvent;

    public void SetObject(UnityEngine.Object obj) {
        setObjectEvent.Invoke(obj);
    }

    public void SetObjectAsGameObject(object obj) {
        setGameObjectEvent.Invoke((GameObject)obj);
    }
}