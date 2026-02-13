using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RemapVector3 : MonoBehaviour {

    [SerializeField] bool invertX = false;
    [SerializeField] bool invertY = false;
    [SerializeField] bool invertZ = false;
    [SerializeField] UnityEvent<Vector3> outputNewVector3;

    [SerializeField] RemapVector3Options Vector2ToVector3MapMode;

    public void Map(Vector3 input) {
        if (invertX) { input.x *= -1; }
        if (invertY) { input.y *= -1; }
        if (invertZ) { input.z *= -1; } 

        if (Vector2ToVector3MapMode == RemapVector3Options.SwapNone) {
            outputNewVector3.Invoke(new Vector3(input.x, input.y, input.z));
        } else if (Vector2ToVector3MapMode == RemapVector3Options.SwapXAndY) {
            outputNewVector3.Invoke(new Vector3(input.y, input.x, input.z));
        } else if (Vector2ToVector3MapMode == RemapVector3Options.SwapYAndZ) {
            outputNewVector3.Invoke(new Vector3(input.x, input.z, input.y));
        } else if (Vector2ToVector3MapMode == RemapVector3Options.SwapXAndZ) {
            outputNewVector3.Invoke(new Vector3(input.z, input.y, input.x));
        }
    }
    enum RemapVector3Options {
        SwapNone,
        SwapXAndY,
        SwapYAndZ,
        SwapXAndZ,
    }
}