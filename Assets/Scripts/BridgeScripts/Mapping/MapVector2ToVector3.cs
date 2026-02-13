using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapVector2ToVector3 : MonoBehaviour
{
    [SerializeField] bool invertX = false;
    [SerializeField] bool invertY = false;
    [SerializeField] UnityEvent<Vector3> outputNewVector3;

    [SerializeField] MapVector2ToVector3Options Vector2ToVector3MapMode;

    public void Map(Vector2 input) {
        if (invertX) { input.x *= -1; }
        if (invertY) { input.y *= -1; }

        if (Vector2ToVector3MapMode == MapVector2ToVector3Options.XToXAndYToY) {
            outputNewVector3.Invoke(new Vector3(input.x, input.y, 0));
        } else if (Vector2ToVector3MapMode == MapVector2ToVector3Options.XToXAndYToZ) {
            outputNewVector3.Invoke(new Vector3(input.x, 0, input.y));
        } else if (Vector2ToVector3MapMode == MapVector2ToVector3Options.XToYAndYToZ) {
            outputNewVector3.Invoke(new Vector3(0, input.x, input.y));
        } else if (Vector2ToVector3MapMode == MapVector2ToVector3Options.XToYAndYToX) {
            outputNewVector3.Invoke(new Vector3(input.y, input.x, 0));
        } else if (Vector2ToVector3MapMode == MapVector2ToVector3Options.XToZAndYToX) {
            outputNewVector3.Invoke(new Vector3(input.y, 0, input.x));
        } else if (Vector2ToVector3MapMode == MapVector2ToVector3Options.XToZAndYToY) {
            outputNewVector3.Invoke(new Vector3(0, input.y, input.x));
        }
    }
    enum MapVector2ToVector3Options {
        XToXAndYToY,
        XToXAndYToZ,
        XToYAndYToZ,
        XToYAndYToX,
        XToZAndYToX,
        XToZAndYToY,
    }
}


