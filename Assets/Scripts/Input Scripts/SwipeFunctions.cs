using UnityEngine;
using UnityEngine.Events;

public class SwipeFunctions : TouchInputManager
{

    [SerializeField] UnityEvent<Vector2> onSwipeDirection, onSwipeDirectionNormalized;
    [SerializeField] UnityEvent<float> onSwipeMagnitude;
    [SerializeField] UnityEvent onSwipe, onSwipeStart;
    [Space]
    [Header("Touch Input Settings")]
    [SerializeField] private float minimumSwipeDistance = 400f;
    [SerializeField] private float maximumSwipeTime = 1.0f;

    [Header("Storage Variables for Touch Inputs")]
    private Vector2 swipeStartPosition;
    private float swipeStartTime;
    private Vector2 swipeDirection;

    public override void OnEnable() {
        base.OnEnable();
        OnStartTouch += SwipeStart;
        OnEndTouch += SwipeEnd;
    }

    public override void OnDisable() {
        base.OnDisable();
        OnStartTouch -= SwipeStart;
        OnEndTouch -= SwipeEnd;
    }


    /// <summary>
    /// Detects if the touch and release points on the screen constitutes a swipe.
    /// </summary>
    private void SwipeStart(Vector2 position, float time) {
        swipeStartPosition = position;
        swipeStartTime = time; 
        onSwipeStart.Invoke();
    }
    private void SwipeEnd(Vector2 position, float time) {
        if (Vector3.Distance(swipeStartPosition, position) >= minimumSwipeDistance &&
            (time - swipeStartTime) <= maximumSwipeTime) {
            swipeDirection = position - swipeStartPosition;
            

            onSwipeDirection.Invoke(swipeDirection);
            onSwipe.Invoke();
            onSwipeDirectionNormalized.Invoke(swipeDirection.normalized);
            onSwipeMagnitude.Invoke(swipeDirection.magnitude);
        }
    }
}
