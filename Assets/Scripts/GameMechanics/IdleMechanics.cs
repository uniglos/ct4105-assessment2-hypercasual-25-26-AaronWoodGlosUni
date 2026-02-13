using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdleMechanics : MonoBehaviour
{
    [Header("Idle mechanics settings")]
    [SerializeField] bool isIdleStatePlaying = false;
    [SerializeField] float stepValue = 0.1f;
    [SerializeField] float stepLowerBound = 0;
    [SerializeField] float stepUpperBound = 1;

    [Space(20)]
    [SerializeField] UnityEvent<float> onIdleUpdate;

    // Update is called once per frame
    void Update()
    {
        if (isIdleStatePlaying) {
            onIdleUpdate.Invoke(stepValue*Time.deltaTime);
        }
    }
    
    public void SetStepValue(float value) {
        stepValue = Mathf.Clamp(value, stepLowerBound, stepUpperBound);
    }
    public void AddToStepValue(float value) {
        stepValue = Mathf.Clamp(stepValue + value, stepLowerBound, stepUpperBound);
    }
    public void SetIdleStatePlay(bool val) {
        isIdleStatePlaying=val;
    }
    public void ToggleIdleStatePlaying() {
        isIdleStatePlaying=!isIdleStatePlaying;
    }

    public void OnValidate() {
        if (stepValue < stepLowerBound) {
            stepValue = stepLowerBound;
        } else if (stepValue > stepUpperBound) {
            stepValue = stepUpperBound;
        }
    }
}
