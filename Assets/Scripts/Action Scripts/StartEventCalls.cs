using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartEventCalls : MonoBehaviour
{
    public UnityEvent GameStartEvent;
    // Start is called before the first frame update
    void Start()
    {
        GameStartEvent.Invoke();
    }

    
}
