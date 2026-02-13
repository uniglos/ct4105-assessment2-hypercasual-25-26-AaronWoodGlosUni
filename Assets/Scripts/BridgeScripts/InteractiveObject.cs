using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    public UnityEvent Interaction;
    public UnityEvent<Vector3> InteractionVec3;

    public void Interacted() {
        Interaction.Invoke();
    }

    public void Interacted(Vector3 vec)
    {
        InteractionVec3.Invoke(vec);
    }
   
}
