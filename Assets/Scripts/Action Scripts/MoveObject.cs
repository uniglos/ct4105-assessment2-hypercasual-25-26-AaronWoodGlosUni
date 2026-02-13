using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MoveObject : MonoBehaviour {

    [SerializeField] UnityEvent onObjectMove, whileObjectMoving, onObjectMoved, OnEndOfMotion;
    [SerializeField] UnityEvent<object> onObjectMovePassGO, whileObjectMovingPassGO, onObjectMovedPassGO;

    public Vector3 StartPosition = Vector3.zero;
    public Vector3 EndPosition = Vector3.zero;
    public float MoveTime = 1;
    public bool Return = false;
    public bool loop = false;

    public void ChangeEndPosition(Vector3 position)
    {        
        EndPosition = position;        
    }

    public void ChangeStartPosition(Vector3 position)
    {
        StartPosition = position;
    }

    public void StartMoving() {
        onObjectMove.Invoke();
        onObjectMovePassGO.Invoke(this);
        StartCoroutine(MoveObjectOverTime(MoveTime, StartPosition, EndPosition));
    }
    public void StopMoving() {
        onObjectMoved.Invoke();
        onObjectMovedPassGO.Invoke(this);
        StopCoroutine(MoveObjectOverTime(MoveTime, StartPosition, EndPosition));
    }

    public void StartMoving(float moveSpeed) {
        onObjectMove.Invoke();
        onObjectMovePassGO.Invoke(this);
        StartCoroutine(MoveObjectOverTime(moveSpeed, StartPosition, EndPosition));
    }
    public void StopMoving(float moveSpeed) {
        onObjectMoved.Invoke();
        onObjectMovedPassGO.Invoke(this);
        StopCoroutine(MoveObjectOverTime(moveSpeed, StartPosition, EndPosition));
    }

    IEnumerator MoveObjectOverTime(float speed, Vector3 StartPos, Vector3 EndPos) {
        float time = 0;
        while (time < 2) {
            whileObjectMoving.Invoke();
            whileObjectMovingPassGO.Invoke(this);
            transform.position = Vector3.Lerp(StartPos, EndPos, time);
            time += Time.deltaTime*speed;
            yield return null;
        }
        OnEndOfMotion.Invoke();
        if (Return) {
            time = 0;
            while (time < 1) {
                transform.position = Vector3.Lerp(EndPos, StartPos, time);
                time += Time.deltaTime*speed;
                yield return null;
            }
            OnEndOfMotion.Invoke();
        }
        if (loop) {
            StartCoroutine(MoveObjectOverTime(speed, StartPos, EndPos));
        }
        
    }
}
