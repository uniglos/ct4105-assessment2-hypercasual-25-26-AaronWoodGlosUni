using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Trigger3D : MonoBehaviour
{

    public string[] Tags;
    
    public UnityEvent<GameObject> TriggerEnter;
    public UnityEvent<GameObject> TriggerStay;
    public UnityEvent<GameObject> TriggerExit;

    

    private void OnTriggerEnter(Collider other)
    {
        
        if (Tags.Contains(other.tag) || Tags.Length == 0)
        {
            TriggerEnter.Invoke(other.gameObject);
            
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Tags.Contains(other.tag) || Tags.Length == 0)
        {
            TriggerStay.Invoke(other.gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (Tags.Contains(other.tag) || Tags.Length == 0)
        {
            TriggerExit.Invoke(other.gameObject);
        }

    }
}
