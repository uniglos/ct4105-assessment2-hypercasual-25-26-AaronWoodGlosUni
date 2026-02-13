using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrueFalseEvents : MonoBehaviour
{
    [SerializeField] UnityEvent ifTrue;
    [SerializeField] UnityEvent ifFalse;

    public void TrueFalseEvent(bool booleanStatement) {
        if (booleanStatement) { ifTrue.Invoke(); }
        else { ifFalse.Invoke(); }
    }
}
