using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TESTScore : MonoBehaviour
{

    public UnityEvent testEvent;
    // Start is called before the first frame update
    void Start()
    {
        testEvent.Invoke();
    }

    
}
