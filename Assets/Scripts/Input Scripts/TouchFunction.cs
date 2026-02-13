using UnityEngine;
using UnityEngine.Events;

public class TouchFunction : TouchInputManager
{
    [SerializeField] UnityEvent<Vector2> onTouchStartPosition, onTouchEndPosition;
    [SerializeField] UnityEvent onTouchStart, onTouchEnd;

    public override void OnEnable() {
        base.OnEnable();
        OnStartTouch += OnTouchStart;
        OnEndTouch += OnTouchEnd;
    }

    public override void OnDisable() {
        base.OnDisable();
        OnStartTouch -= OnTouchStart;
        OnEndTouch -= OnTouchEnd;
    }

    private void OnTouchStart(Vector2 position, float time) {
        onTouchStart.Invoke(); onTouchStartPosition.Invoke(position);
    }
    private void OnTouchEnd(Vector2 position, float time) {
        onTouchEnd.Invoke(); onTouchEndPosition.Invoke(position);
    }
}
