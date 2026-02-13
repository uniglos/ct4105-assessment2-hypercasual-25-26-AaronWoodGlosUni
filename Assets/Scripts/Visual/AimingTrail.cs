using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class AimingTrail : MonoBehaviour {

    [SerializeField] LineRenderer ln;
    [SerializeField] float trailPower = 0.05f;

    Vector3 startPos;

    public void StartAiming(Vector3 startPosition) {
        startPos = startPosition;
        if (ln != null) {
            if (ln.positionCount != 2) {
                ln.positionCount = 2;
            }
            ln.SetPosition(0, startPosition);
            ln.SetPosition(1, startPosition);
        }
    }
    public void WhileAiming(Vector3 input) {
        Vector3  offset = new Vector3(0,-transform.position.y,0);
        if (ln != null && ln.positionCount>0) {
            ln.SetPosition(0, transform.position);
            ln.SetPosition(1, startPos - (input * trailPower)- offset);
        }
    }

    private void Update()
    {
        
    }

    public void EndAiming() {
        if (ln != null) {
            ln.positionCount = 0;
        }
    }
}