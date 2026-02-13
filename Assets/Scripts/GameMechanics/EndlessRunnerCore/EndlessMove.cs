using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class EndlessMove : MonoBehaviour
{
    //Put this on the parent object of a road segment so that it starts moving once it has spawned in

    EndlessRunner _instance;
    public float speed = 1;

    private void Start()
    {
        _instance = EndlessRunner.Instance;
    }

   

    private void FixedUpdate()
    {
        if (_instance.Playing == true)
        {
            transform.position += Vector3.back * speed;
        }
    }
}
