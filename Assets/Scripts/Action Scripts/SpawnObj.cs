using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnObj : MonoBehaviour
{
    public Transform Parent;
    public GameObject SpawnObject;
    public Vector3 SpawnLocation;
    public Quaternion SpawnRotation;


    public void SpawnObjectInScene()
    {
        if (Parent == null)
        {
            Instantiate(SpawnObject, SpawnLocation, SpawnRotation, null);
        }
        else
        {
            Instantiate(SpawnObject, SpawnLocation, SpawnRotation, Parent);
        }
    }


    public void SpawnObjectAt3D(Vector3 SpawnPos)
    {

        Instantiate(SpawnObject, SpawnPos, SpawnRotation, null);
    }
}
