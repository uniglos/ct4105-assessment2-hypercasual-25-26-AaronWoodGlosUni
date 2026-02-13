using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivateChildren : MonoBehaviour
{
    UnityEvent OnChildActivate;

    public static void ActivateAllChildren(GameObject parent)
    {
        if (parent.transform.childCount > 0)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }


}
