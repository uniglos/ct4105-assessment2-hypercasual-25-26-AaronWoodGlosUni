using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EndlessRunTrigger : MonoBehaviour
{
    //Put this script on the trigger point in the middle of each road segment
    EndlessRunner _instance;
    public string[] Tags;


    private void Start()
    {
        _instance = EndlessRunner.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Tags.Contains(other.tag) || Tags.Length == 0)
        {
            _instance.InstantiateRoad();
        }
    }

}
