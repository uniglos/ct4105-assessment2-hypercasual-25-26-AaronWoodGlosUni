using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PinchFunction : TouchInputManager {

    [SerializeField] UnityEvent<float> onPinchStartFloat, onPinchEndFloat, whilePinchingScaleProportion, whilePinchingScaleDelta;
    [SerializeField] UnityEvent onPinchStart, onPinchEnd, whilePinching;

    bool isPinching;
    float startingDistance;
    float currentScale;
    float previousScale;

    private void Update() {
        if (isPinching) {
            currentScale = Vector2.Distance(touchControls.TouchScreen.TouchPosition1.ReadValue<Vector2>(),
        touchControls.TouchScreen.TouchPosition2.ReadValue<Vector2>()) / startingDistance;
            if (currentScale - previousScale != 0) {
                whilePinchingScaleDelta.Invoke(currentScale - previousScale);
                whilePinchingScaleProportion.Invoke(currentScale);
                whilePinching.Invoke();
            }
            previousScale = currentScale;
        };
    }

    public override void OnEnable() {
        base.OnEnable();
        OnStartSecondTouch += StartPinch;
        OnEndSecondTouch += EndPinch;
    }

    public override void OnDisable() {
        base.OnDisable();
        OnStartSecondTouch -= StartPinch;
        OnEndSecondTouch -= EndPinch;
    }

    public void StartPinch(Vector2 pos1, Vector2 pos2, float time) {
        onPinchStartFloat.Invoke(1); onPinchStart.Invoke();
        isPinching = true;
        startingDistance = Vector2.Distance(pos1, pos2);
    }
    public void EndPinch(Vector2 pos1, Vector2 pos2, float time) {
        onPinchEndFloat.Invoke(1); onPinchEnd.Invoke();
        isPinching = false;
    }
}
