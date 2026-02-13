using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-1)]
public class TowrGameManager : MonoBehaviour {

    [SerializeField] bool randomSpawnerPerClick = false;
    [SerializeField] bool randomFirstSpawner = true;
    [SerializeField] public MovingCube firstCube;
    [SerializeField] CubeSpawner[] cubeSpawner;

    [SerializeField] public float minHangoverForPerfectScore = 0.1f;
    [SerializeField] public float maxCubeTravelDistance = 2;
    [SerializeField] public float moveSpeed = 1f;

    [SerializeField] Camera Camera;

    [SerializeField] UnityEvent onGameStart, onCubeStopped, onGameOver;

    int currentSpawnerID=0;
    bool isGameOver = false;


    private void Awake() {
        MovingCube.gameManager = this;
        MovingCube.lastCube = firstCube;
        if (randomFirstSpawner) {
            currentSpawnerID = UnityEngine.Random.Range(0, cubeSpawner.Length);
        }
    }

    public void StopCube() {
       // if (!isGameOver)
        //{
            if (MovingCube.lastCube != MovingCube.currentCube)
            {
                MovingCube.currentCube.Stop();
                onCubeStopped.Invoke();
            }
            else
            {
                onGameStart.Invoke();
            }
            if (!isGameOver) { 
            cubeSpawner[currentSpawnerID].SpawnCube();
            Camera.transform.position += new Vector3(0, MovingCube.currentCube.transform.localScale.y, 0);

            if (!randomSpawnerPerClick)
            {
                currentSpawnerID = currentSpawnerID >= cubeSpawner.Length - 1 ? 0 : 1;
            }
            else
            {
                currentSpawnerID = UnityEngine.Random.Range(0, cubeSpawner.Length);
            }
        }
        //}
    }

    public void StopGame() {
        isGameOver = true;
        onGameOver.Invoke();
        
    }
}
