using UnityEngine;
using UnityEngine.Events;

public class TapFunction : TouchInputManager {

    [Space]
    [Header("Touch Input Settings")]
    [SerializeField] private float maximumTapTime = 1.0f;
    [SerializeField] private float minimumTapDistance = 200.0f;

    [Header("Storage Variables for Touch Inputs")]
    private Vector2 startTapPos;
    private float startTapTime;

    [SerializeField] UnityEvent<Vector2> onTapPosition;
    [SerializeField] UnityEvent onTap;

    public override void OnEnable() {
        base.OnEnable();
        OnStartTouch += TapStart;
        OnEndTouch += TapEnd;
    }

    public override void OnDisable() {
        base.OnDisable();
        OnStartTouch -= TapStart;
        OnEndTouch -= TapEnd;
    }

    /// <summary>
    /// Detects if the touch and release points on the screen constitutes a swipe.
    /// </summary>
    private void TapStart(Vector2 position, float time) {
        startTapTime = time; startTapPos = position;
    }
    private void TapEnd(Vector2 position, float time) {
        if (Vector3.Distance(startTapPos, position) <= minimumTapDistance && (time-startTapTime) <= maximumTapTime) {
            onTapPosition.Invoke(position);
            onTap.Invoke();            
        }
    }
}
