using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectRail : MonoBehaviour
{
    [SerializeField] Vector3 fixedStart;
    [SerializeField] Vector3 fixedEnd;

    [SerializeField] float stopForce = 1;

    Vector2 currentVel;
    Vector3 newPos;

    public void Vector2Rail(Vector2 dir) {
        StartCoroutine(Rail(new Vector3(dir.x,dir.y,0)));
    }
    public void Vector3Rail(Vector3 dir) {
        StartCoroutine(Rail(dir));
    }

    private IEnumerator Rail(Vector3 dir) {
        currentVel = dir;
        print("swipe");
        
        while (true) {
            currentVel = new Vector3(dir.x * Time.deltaTime, dir.y * Time.deltaTime) / stopForce;
            if (fixedStart.x < fixedEnd.x) {
                newPos = new Vector3(Mathf.Clamp(transform.position.x + dir.x, fixedStart.x, fixedEnd.x), 0, 0);
            } else {
                newPos = new Vector3(Mathf.Clamp(transform.position.x + dir.x, fixedEnd.x, fixedStart.x), 0, 0);
            }

            if (fixedStart.y < fixedEnd.y) {
                newPos += new Vector3(0, Mathf.Clamp(transform.position.y + dir.y, fixedStart.y, fixedEnd.y), 0);
            } else {
                newPos += new Vector3(0, Mathf.Clamp(transform.position.y + dir.y, fixedEnd.y, fixedStart.y), 0);
            }

            if (fixedStart.z < fixedEnd.z) {
                newPos += new Vector3(0, 0, Mathf.Clamp(transform.position.z + dir.z, fixedStart.z, fixedEnd.z));
            } else {
                newPos += new Vector3(0, 0, Mathf.Clamp(transform.position.z + dir.z, fixedEnd.z, fixedStart.z));
            }

            transform.position = newPos;

            if (currentVel.magnitude < 0.5f) {
                break;
            }
            yield return null;
        }
        yield return null;
    }
}
