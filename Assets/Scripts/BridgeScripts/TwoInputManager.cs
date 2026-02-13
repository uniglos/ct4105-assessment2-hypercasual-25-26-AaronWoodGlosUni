using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwoInputManager : MonoBehaviour
{
   public GameButton inputOne, inputTwo;

    public UnityEvent TwoInputs;

    public void CheckInputsAND()
    {
        if( inputOne.Active && inputTwo.Active)
        {
            TwoInputs.Invoke();
        }
    }

    public void CheckInputsOR()
    {
        if( inputOne.Active ||  inputTwo.Active)
        {
            TwoInputs.Invoke();
        }
    }

    public void CheckInputXOR()
    {
        if(inputOne.Active ^ inputTwo.Active)
        {
            TwoInputs.Invoke();
        }

    }
        
}
