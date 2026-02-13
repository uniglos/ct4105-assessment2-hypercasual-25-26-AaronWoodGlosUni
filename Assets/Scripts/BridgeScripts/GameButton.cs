using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameButton : MonoBehaviour
{
    public bool Active;
    public UnityEvent ButtonActivated;
    public UnityEvent ButtonDisactivated;

    public void ActivateButton()
    {
        if (!Active)
        {
            Active = true;
            ButtonActivated.Invoke();
        }
    }

    public void buttonDisable()
    {
        if (Active)
        {
            Active = false;
            ButtonDisactivated.Invoke();
        }
    }

}
