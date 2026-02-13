using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GenerateInArea : MonoBehaviour
{
    [SerializeField] private GameobjectGeneration[] GameobjectSpawnObjects;
    [SerializeField] private Transform generatedObjectsParent;

    [SerializeField] private Vector3 gizmoCornerSize = new Vector3(1,1,1);
    [SerializeField] private Color gizmoColor = Color.cyan;
    [SerializeField] private Color gizmoCornerColor = Color.blue;

    [SerializeField] private Vector3 corner1;
    [SerializeField] private Vector3 corner2;

    [HideInInspector] public List<GameObject> generatedObjects;

    [SerializeField] UnityEvent onGeneratedObjectsZero;
    [SerializeField] UnityEvent onRemovedObject;

    //Random values
    private float randomProb;
    private float checkSpawnProbability;
    private float prevCheckSpawnProbability;

    //Validate
    private float checkCount;
    private float prevCheckCount;

    float totalProbability { get { return GameobjectSpawnObjects.Sum(i => i.relativeSpawnProbability); } }

    public void RemoveItemFromList(GameObject go) {
        if (generatedObjects.Contains(go)) { generatedObjects.Remove(go); onRemovedObject.Invoke(); }
        if (generatedObjects.Count == 0) { onGeneratedObjectsZero.Invoke(); }
    }

    public void SpawnMultipleObjects(int spawnAmount) {
        for (int i = 0; i < spawnAmount; i++) {
            RandomSpawn();
        }
    }

    public void RandomSpawn() {
        randomProb = UnityEngine.Random.Range(0,1.0f);
        checkSpawnProbability=0;
        for (int i = 0; i < GameobjectSpawnObjects.Length; i++) {
            prevCheckSpawnProbability = checkSpawnProbability;
            checkSpawnProbability += GameobjectSpawnObjects[i].relativeSpawnProbability;
            if (randomProb > prevCheckSpawnProbability && randomProb < checkSpawnProbability) {
                generatedObjects.Add(Instantiate(GameobjectSpawnObjects[i].go, GetRandomCoordinate(), Quaternion.identity, gameObject.transform));
                generatedObjects[generatedObjects.Count-1].SetActive(true);
                break;
            }
        }
    }

    public Vector3 GetRandomCoordinate() {
        return new Vector3(corner1.x + UnityEngine.Random.Range(0,corner2.x), corner1.y + UnityEngine.Random.Range(0, corner2.y), corner1.z + UnityEngine.Random.Range(0, corner2.z)); 
    }

    private void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(gameObject.transform.position + ((corner2 - corner1) / 2) + corner1, corner2 - corner1);
        Gizmos.color = gizmoCornerColor;
        Gizmos.DrawWireCube(corner1, gizmoCornerSize);
        Gizmos.DrawWireCube(corner2, gizmoCornerSize);
    }

    private void OnValidate() {
        checkCount = 0;
        prevCheckCount = 0;

        for (int i = 0; i < GameobjectSpawnObjects.Length; i++) {
            checkCount += GameobjectSpawnObjects[i].relativeSpawnProbability;
            if (checkCount > 1) { 
                GameobjectSpawnObjects[i].relativeSpawnProbability = checkCount;
                GameobjectSpawnObjects[i].relativeSpawnProbability = 1 - prevCheckCount;
                break;
            }
            prevCheckCount = checkCount;
        }
        if (checkCount < 1 && GameobjectSpawnObjects.Length>0) {
            GameobjectSpawnObjects[GameobjectSpawnObjects.Length - 1].relativeSpawnProbability += 1-checkCount;
        }

        if (generatedObjectsParent != null) {
            generatedObjectsParent = this.transform;
        }
    }
}

[Serializable]
public struct GameobjectGeneration {
    public GameObject go;
    public float relativeSpawnProbability;
}