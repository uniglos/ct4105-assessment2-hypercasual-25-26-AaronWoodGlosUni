using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RotateObject : MonoBehaviour {

    [Header("Rotate over time")]
    [Space]
    public Vector3 StartRotation = Vector3.zero;
    public Vector3 EndRotation = Vector3.zero;
    public float RotationTime = 1;
    public bool Return = false;
    public bool loop = false;

    [SerializeField] UnityEvent onObjectRotate, whileObjectRotating, onObjectRotated;
    [SerializeField] UnityEvent<object> onObjectRotatePassGO, whileObjectRotatingPassGO, onObjectRotatedPassGO;

    [Space]
    [Header("Rotate Instantly")]
    [SerializeField] float RotPower = 0.5f;
    [SerializeField] Vector3 LowerEulerAngleBound;
    [SerializeField] Vector3 UpperEulerAngleBound;

    Vector3 unofficialEulerAngles;

    private void Start() {
        SetUnofficialEulerAngles();
    }

    public void StartRotate() {
        onObjectRotate.Invoke();
        onObjectRotatePassGO.Invoke(this);
        StartCoroutine(RotateObjectOverTime(RotationTime, StartRotation, EndRotation));
    }
    public void StopRotate() {
        onObjectRotated.Invoke();
        onObjectRotatedPassGO.Invoke(this);
        StopCoroutine(RotateObjectOverTime(RotationTime, StartRotation, EndRotation));
    }


    public void StartRotate(float rotSpeed) {
        onObjectRotate.Invoke();
        onObjectRotatePassGO.Invoke(this);
        StartCoroutine(RotateObjectOverTime(rotSpeed, StartRotation, EndRotation));
    }

    IEnumerator RotateObjectOverTime(float speed, Vector3 startRot, Vector3 endRot) {
        float time = 0;
        while (time < speed) {
            whileObjectRotating.Invoke();
            whileObjectRotatingPassGO.Invoke(this);
            transform.eulerAngles = Vector3.Lerp(startRot, endRot, time);
            SetUnofficialEulerAngles();
            time += Time.deltaTime;
            yield return null;
        }
        if (Return) {
            time = 0;
            while (time < 1) {
                transform.eulerAngles = Vector3.Lerp(endRot, startRot, time);
                SetUnofficialEulerAngles();
                time += Time.deltaTime;
                yield return null;
            }
        }
        if (loop) {
            StartCoroutine(RotateObjectOverTime(speed, startRot, endRot));
        }
    }

    public void InstantRotation(Vector2 currentRot) {
        if (currentRot != Vector2.zero) {
            InstantRotation(new Vector3(currentRot.x, currentRot.y, 0));
        }
    }
    public void InstantRotation(Vector3 currentRot) {
        
        unofficialEulerAngles += currentRot * RotPower;
        unofficialEulerAngles = new Vector3(
            Mathf.Clamp(unofficialEulerAngles.x, LowerEulerAngleBound.x, UpperEulerAngleBound.x),
            Mathf.Clamp(unofficialEulerAngles.y, LowerEulerAngleBound.y, UpperEulerAngleBound.y),
            Mathf.Clamp(unofficialEulerAngles.z, LowerEulerAngleBound.z, UpperEulerAngleBound.z)
            );
        gameObject.transform.eulerAngles = unofficialEulerAngles;
        
    }

    private void OnValidate() {
        if (LowerEulerAngleBound.x > UpperEulerAngleBound.x) { LowerEulerAngleBound.x = UpperEulerAngleBound.x; }
        if (LowerEulerAngleBound.y > UpperEulerAngleBound.y) { LowerEulerAngleBound.y = UpperEulerAngleBound.y; }
        if (LowerEulerAngleBound.z > UpperEulerAngleBound.z) { LowerEulerAngleBound.z = UpperEulerAngleBound.z; }
    }

    public void SetUnofficialEulerAngles() {
        unofficialEulerAngles = transform.eulerAngles;
        if (unofficialEulerAngles.x > 180) { unofficialEulerAngles.x -= 360; }
        if (unofficialEulerAngles.y > 180) { unofficialEulerAngles.y -= 360; }
        if (unofficialEulerAngles.z > 180) { unofficialEulerAngles.z -= 360; }
    }
}
