using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoubleTapFunction : TouchInputManager {

    [Header("Touch Input Settings")]
    [SerializeField] private float doubleTapMaximumTime = .3f;
    [SerializeField] private float doubleTapMinimumDistance = 200;

    //Temporary storage variables
    private float startDoubleTapTime;
    private Vector2 startDoubleTapPos;
    private bool isDoubleTapping = false;

    [SerializeField] UnityEvent<Vector2> onDoubleTapPosition;
    [SerializeField] UnityEvent onDoubleTap;

    public override void OnEnable() {
        base.OnEnable();
        OnStartTouch += DoubleTapStart;
    }

    public override void OnDisable() {
        base.OnDisable();
        OnStartTouch -= DoubleTapStart;
    }

    /// <summary>
    /// Invokes the double tap event if the player taps the screen in quick succession
    /// </summary>
    private void DoubleTapStart(Vector2 position, float time) {
        if (!isDoubleTapping) {
            startDoubleTapTime = time;
            startDoubleTapPos = position;
            isDoubleTapping = true;
            StartCoroutine(DoubleTap());
        } else {
            if ((time - startDoubleTapTime <= doubleTapMaximumTime) && (Vector2.Distance(startDoubleTapPos, position) < doubleTapMinimumDistance)) { onDoubleTapPosition.Invoke(position); onDoubleTap.Invoke(); }
            isDoubleTapping = false;
        }
    }
    /// <summary>
    /// A EventTimer for the doubletap event.
    /// </summary>
    private IEnumerator DoubleTap() {
        float delta = 0;
        while (delta <= doubleTapMaximumTime && isDoubleTapping) {
            delta += Time.deltaTime;
            yield return null;
        }
        if (delta >= doubleTapMaximumTime) {
            isDoubleTapping = false;
        }
    }
}
