using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour {
    public static TowrGameManager gameManager { get; set; }
    public static MovingCube currentCube { get; private set; }
    public static MovingCube lastCube { get; set; }
    public MoveDirection MoveDirection { get; set; }

    [SerializeField] float directionMult = 1;
    private float hangover;

    float startCoord;
    float endCoord;

    private void Start() {
        if (MoveDirection == MoveDirection.Z) {
            startCoord = transform.position.z; endCoord = startCoord - gameManager.maxCubeTravelDistance;
            if (endCoord > startCoord) { startCoord = endCoord; endCoord = transform.position.z; }
        }
        if (MoveDirection == MoveDirection.X) {
            startCoord = transform.position.x; endCoord = startCoord - gameManager.maxCubeTravelDistance;
            if (endCoord > startCoord) { startCoord = endCoord; endCoord = transform.position.x; }
        }
    }

    private void OnEnable() {
        
        if (currentCube == null) {
            lastCube = gameManager.firstCube;
        } else {
            lastCube = currentCube;
        }
        currentCube = this;
        if (currentCube != lastCube) {
            directionMult = 1;
        }
        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3(lastCube.transform.localScale.x, transform.localScale.y, lastCube.transform.localScale.z);
    }

    private Color GetRandomColor() {
        return new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    internal void Stop() {
        directionMult = 0;
        hangover = GetHangover();

        float max = MoveDirection == MoveDirection.Z ? lastCube.transform.localScale.z : lastCube.transform.localScale.x;
        if (Mathf.Abs(hangover) >= max) {
            lastCube = null;
            currentCube = null;
            
            gameManager.StopGame();            
        }

        if (Mathf.Abs(hangover) < gameManager.minHangoverForPerfectScore) {
            currentCube.transform.position = lastCube.transform.position + new Vector3(0, (lastCube.transform.localScale.y / 2) + (currentCube.transform.localScale.y / 2), 0);
            lastCube = currentCube;
        } else {
            if (MoveDirection == MoveDirection.Z) {
                SplitCubeOnZ(hangover);
            } else {
                SplitCubeOnX(hangover);
            }
        }
    }

    private float GetHangover() {
        if (MoveDirection == MoveDirection.Z) {
            return transform.position.z - lastCube.transform.position.z;
        } else {
            return transform.position.x - lastCube.transform.position.x;
        }
    }

    private void SplitCubeOnX(float hang)
    {        
        float direction = hang > 0 ? 1f : -1f;
        float newXSize = 0;
        float fallingBlockSize= transform.localScale.x;

        float newXPosition = transform.position.x;

        if (lastCube != null)
        {
            
            newXSize = lastCube.transform.localScale.x - Mathf.Abs(hang);
            fallingBlockSize = transform.localScale.x - newXSize;

            newXPosition = lastCube.transform.position.x + (hang / 2);

        }

        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;
        if (lastCube == null) Destroy(transform.gameObject);
        SpawnDropCube(fallingBlockXPosition, fallingBlockSize);


    }
    private void SplitCubeOnZ(float hang)
    {

        float direction = hang > 0 ? 1f : -1f;
        float newZSize = 0;
        float fallingBlockSize = transform.localScale.z;

        float newZPosition = transform.position.z;

        if (lastCube != null)
        {
            newZSize = lastCube.transform.localScale.z - Mathf.Abs(hang);
            fallingBlockSize = transform.localScale.z - newZSize;

            newZPosition = lastCube.transform.position.z + (hang / 2);
        }


        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newZSize);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZPosition);

        float cubeEdge = transform.position.z + (newZSize / 2f * direction);
        float fallingBlockZPosition = cubeEdge + fallingBlockSize / 2f * direction;
        if (lastCube == null) Destroy(transform.gameObject);
        SpawnDropCube(fallingBlockZPosition, fallingBlockSize);

    }

    private void SpawnDropCube(float fallingBlockPosition, float fallingBlockSize) {

        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z) {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, fallingBlockSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, fallingBlockPosition);
        } else {
            cube.transform.localScale = new Vector3(fallingBlockSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(fallingBlockPosition, transform.position.y, transform.position.z);
        }

        cube.AddComponent<Rigidbody>(); cube.GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color;

        lastCube = currentCube;
        Destroy(cube.gameObject, 1f);
    }

    // Update is called once per frame
    private void Update() {
        if (directionMult != 0) {
            if (MoveDirection == MoveDirection.Z) {
                transform.position += transform.forward * Time.deltaTime * gameManager.moveSpeed * directionMult;
                if (transform.position.z < endCoord) { directionMult = -1; }
                if (transform.position.z > startCoord) { directionMult = 1; }

            } else {
                transform.position += transform.right * Time.deltaTime * gameManager.moveSpeed * directionMult;
                if (transform.position.x < endCoord) directionMult = -1;
                if (transform.position.x > startCoord) directionMult = 1;
            }
        }
    }
}