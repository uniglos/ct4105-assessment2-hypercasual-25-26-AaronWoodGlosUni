using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class DebugMessage : MonoBehaviour {
    public void PrintMessage(string message) {
        Debug.Log(message+"{DebugScript}");
    }

    public void PrintValue(float val) {
        Debug.Log(val + "{DebugScript}");
    }
    public void PrintVector(Vector2 val) {
        Debug.Log(val + "{DebugScript}");
    }
    public void PrintVector(Vector3 val) {
        Debug.Log(val + "{DebugScript}");
    }
}