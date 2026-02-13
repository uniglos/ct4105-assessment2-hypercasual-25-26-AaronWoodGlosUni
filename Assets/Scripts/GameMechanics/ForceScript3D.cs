using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceScript3D : MonoBehaviour {

    [SerializeField] Rigidbody rb;

    public void SetForce(Vector3 val) {
        rb.AddForce(val);
    }
    public void AddForce(Vector3 val) {
        rb.linearVelocity = val;
    }
}
