using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PanZoomCamera : MonoBehaviour
{

    [SerializeField] Vector3 lowerBound;
    [SerializeField] Vector3 upperBound;
    [SerializeField] Camera cam;

    private void Start() {
        if (cam == null) {
            cam = GetComponent<Camera>();
        }
    }

    float height { 
        get {
            return cam.orthographicSize; 
        } 
    }
    float width {
        get {
            return height * cam.aspect;
        }
    }

    [SerializeField] bool invertPan = false;
    [SerializeField, Range(1, 30)] float panPower = 5;

    [SerializeField] bool invertZoom = false;
    [SerializeField, Range(1,30)] float zoomPower = 10;
    [SerializeField] float zoomOutMin = 1;
    [SerializeField] float zoomOutMax = 8;

    Vector3 touchStart;
    float startScale;
    float x;
    float y;
    float z;

    private void Update() {
        ClampPosition();
    }

    public void StartPan() {
        touchStart = cam.transform.position;
    }

    public void PanAroundStartPoint(Vector2 touchPos) {
        if (!invertPan) {
            cam.transform.position = touchStart + (new Vector3(touchPos.x, touchPos.y, 0) * panPower * 0.01f);
        } else {
            cam.transform.position = touchStart - (new Vector3(touchPos.x, touchPos.y, 0) * panPower * 0.01f);
        }
    }
    public void PanByFrame(Vector2 touchPos) {
        if (!invertPan) {
            cam.transform.position += new Vector3(touchPos.x, touchPos.y, 0) * panPower * 0.01f;
        } else {
            cam.transform.position -= new Vector3(touchPos.x, touchPos.y, 0) * panPower * 0.01f;
        }
    }

    public void StartZoom() {
        startScale = cam.orthographicSize;
    }

    public void zoom(float increment) {
        if (!invertZoom) {
            cam.orthographicSize = Mathf.Clamp(startScale * increment * zoomPower * 0.1f, zoomOutMin, zoomOutMax);
        } else {
            cam.orthographicSize = Mathf.Clamp(startScale * (1/increment) * zoomPower * 0.1f, zoomOutMin, zoomOutMax);
        }
    }

    public void ClampPosition() {
        x = Mathf.Clamp(cam.transform.position.x,lowerBound.x+width,upperBound.x-width);
        y = Mathf.Clamp(cam.transform.position.y,lowerBound.y+height,upperBound.y-height);
        z = Mathf.Clamp(cam.transform.position.z,lowerBound.z,upperBound.z);
        cam.transform.position = new Vector3(x,y,z);
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, zoomOutMin, zoomOutMax);
    }
    private void OnValidate() {
        if (lowerBound.x > upperBound.x) { lowerBound.x = upperBound.x; }
        if (lowerBound.y > upperBound.y) { lowerBound.y = upperBound.y; }
        if (lowerBound.z > upperBound.z) { lowerBound.z = upperBound.z; }
    }
}

