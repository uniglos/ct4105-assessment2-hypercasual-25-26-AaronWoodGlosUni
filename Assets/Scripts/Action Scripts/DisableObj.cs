using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisableObj : MonoBehaviour
{

    public void Disable(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Enable(GameObject obj)
    {
        obj.SetActive(true);
    }

   
}
