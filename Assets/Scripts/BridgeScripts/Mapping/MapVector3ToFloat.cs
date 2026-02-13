using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class MapVector3ToFloat : MonoBehaviour {

    [SerializeField] bool invertX = false;
    [SerializeField] bool invertY = false;
    [SerializeField] bool invertZ = false;

    [SerializeField] UnityEvent<float> outputFloat;

    [SerializeField] Vector3Options options;

    public void Map(Vector3 input) {
        if (invertX) { input.x *= -1; }
        if (invertY) { input.y *= -1; }
        if (invertZ) { input.z *= -1; }

        if (options == Vector3Options.getX) {
            outputFloat.Invoke(input.x);
        } else if (options == Vector3Options.getY) {
            outputFloat.Invoke(input.y);
        } else if (options == Vector3Options.getZ) {
            outputFloat.Invoke(input.z);
        }
    }

    enum Vector3Options {
        getX,
        getY,
        getZ,
    }
}
