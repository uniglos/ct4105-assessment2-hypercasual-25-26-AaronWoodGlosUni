using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private MovingCube cubePrefab;
    [SerializeField] private MoveDirection moveDirection;

    public void SpawnCube() {
        var cube = Instantiate(cubePrefab);

        if (MovingCube.lastCube != null && MovingCube.lastCube.gameObject != MovingCube.gameManager.firstCube) {

            float x = moveDirection == MoveDirection.X ? transform.position.x : MovingCube.lastCube.transform.position.x;
            float z = moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.lastCube.transform.position.z;

            cube.transform.position = new Vector3(x,
                MovingCube.lastCube.transform.position.y + (cubePrefab.transform.localScale.y/2) + (MovingCube.lastCube.transform.localScale.y/2),
                z);
        }

        cube.MoveDirection = moveDirection;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, cubePrefab.transform.localScale);
    }

}

public enum MoveDirection {
    X,
    Z,
}