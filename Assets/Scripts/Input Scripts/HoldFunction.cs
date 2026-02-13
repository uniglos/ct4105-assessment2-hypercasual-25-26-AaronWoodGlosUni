using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HoldFunction : TouchInputManager {

    [Header("Touch Input Settings")]
    [SerializeField] private float minimumHoldTime = .5f;
    [SerializeField] private float maximumHoldDistance = 50.0f;

    [SerializeField] UnityEvent onHoldStart, onHoldEnd, whileHolding;
    [SerializeField] UnityEvent<Vector2> onHoldStartPosition, onHoldEndPosition, whileHoldingPositionDelta, whileHoldingChangeInPosition;
    [SerializeField] UnityEvent<Vector2> onHoldEndChangeInPos;

    [Space]
    [Header("Storage Variables for Touch Inputs")]
    #region Temp Variables
    IEnumerator holding = null;
    Vector2 startHoldPosition = Vector2.zero;
    Vector2 currentHoldPos;
    Vector2 touchPosition;
    Vector2 tempHoldPos;
    float startHoldTime;
    float delta = 0;
    float movedDistance;
    bool isHolding;
    #endregion

    /// <summary>
    /// If the player is holding the screen, 
    /// </summary>
    private void Update() {
        if (isHolding) {
            try {
                whileHoldingPositionDelta.Invoke(touchControls.TouchScreen.TouchPosition1.ReadValue<Vector2>() - tempHoldPos);
                tempHoldPos = touchControls.TouchScreen.TouchPosition1.ReadValue<Vector2>();
                whileHoldingChangeInPosition.Invoke(tempHoldPos - startHoldPosition);
            }
            catch { }
            whileHolding.Invoke();
            currentHoldPos = tempHoldPos;
        }
    }

    public override void OnEnable() {
        base.OnEnable();
        OnStartTouch += HoldTouchStart;
        OnEndTouch += HoldTouchEnd;
    }

    public override void OnDisable() {
        base.OnDisable();
        isHolding = false;
        OnStartTouch -= HoldTouchStart;
        OnEndTouch -= HoldTouchEnd;
        StopHold();
    }

    /// <summary>
    /// If the player holds down on the screen, it will trigger an event
    /// </summary>
    private void HoldTouchStart(Vector2 position, float time) {
        if (position != Vector2.zero) {
            tempHoldPos = position;
            startHoldPosition = position;
            startHoldTime = time;
            holding = HoldTouchTimer();
            StartCoroutine(holding);
        }
    }

    private void HoldTouchEnd(Vector2 position, float time) {
        if (isHolding) {
            isHolding = false;
            onHoldEnd.Invoke();
            onHoldEndPosition.Invoke(position);
            onHoldEndChangeInPos.Invoke(position - startHoldPosition);
        }
        if (holding != null) {
            StopCoroutine(holding);
        }
    }

    private IEnumerator HoldTouchTimer() {
        delta = 0;
        while (delta <= minimumHoldTime && touchControls.TouchScreen.TouchPress1.IsPressed()) {
            delta += Time.deltaTime;
            touchPosition = touchControls.TouchScreen.TouchPosition1.ReadValue<Vector2>();
            movedDistance = Vector2.Distance(startHoldPosition, touchPosition);

            if (delta >= minimumHoldTime && movedDistance <= maximumHoldDistance && !isHolding) {
                onHoldStartPosition.Invoke(touchPosition);
                onHoldStart.Invoke();
                currentHoldPos = touchPosition;
                isHolding = true;
                break;
            } else if (movedDistance > maximumHoldDistance) { isHolding = false; break; }
            yield return null;
        }
    }

    private void StopHold() {
        if (holding != null) {
            StopCoroutine(holding);
            isHolding = false;
        }
    }
}