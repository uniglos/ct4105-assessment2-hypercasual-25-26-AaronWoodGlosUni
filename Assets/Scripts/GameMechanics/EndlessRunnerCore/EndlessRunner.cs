using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EndlessRunner : MonoBehaviour
{
    /// <summary>
    /// Put this script on a Manager object.
    /// WARNING. Only one instance of this script can exist in a scene.
    /// </summary>

    public bool Playing = true;
    public UnityEvent OnRoadSpawn;
    public UnityEvent onGameOver;
    public static EndlessRunner Instance;
    List<GameObject> pathPrefabs;
    public Vector3 spawnPos;
    GameObject CurrentRoad;
    //Singleton
    private void Awake()
    {
        pathPrefabs = new List<GameObject>();   
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else { 
            Instance = this;
        }
        
        if (transform.childCount > 0)
        {            
            for (int i = 0; i < transform.childCount; i++)
            {                
                pathPrefabs.Add(transform.GetChild(i).gameObject);                
            }
            CurrentRoad = transform.GetChild(0).gameObject;
        }


    }

    public void GameOver(GameObject other)
    {        
        Playing = false;
        onGameOver.Invoke();
        
    }
    public void InstantiateRoad()
    {
        pathPrefabs.Remove(CurrentRoad);
        OnRoadSpawn.Invoke();
        GameObject Path = pathPrefabs[Random.Range(0, pathPrefabs.Count)];
        Path.SetActive(true);
        ActivateChildren.ActivateAllChildren(Path);
        Path.transform.position = spawnPos;

        pathPrefabs.Add(CurrentRoad);
        CurrentRoad = Path;
        
    }
}
