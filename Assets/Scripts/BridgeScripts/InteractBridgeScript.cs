using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractBridgeScript : MonoBehaviour, IInteract
{
    [SerializeField] UnityEvent InteractEvent;

    public void Interact() {
        InteractEvent.Invoke();
    }
}
