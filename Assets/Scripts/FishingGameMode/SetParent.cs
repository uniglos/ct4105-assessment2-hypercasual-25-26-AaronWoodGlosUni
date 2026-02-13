using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParent : MonoBehaviour
{

    public void SetTransformParent(GameObject parent)
    {
        transform.parent = parent.transform;
    }

    public void SetThisAsParent(GameObject child)
    {
        child.transform.parent = transform;
    }
}
