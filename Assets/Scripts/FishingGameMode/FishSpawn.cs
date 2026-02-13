
using System.Collections.Generic;
using UnityEngine;

public class FishSpawn : MonoBehaviour
{
    Vector2 spawnRange;
    GameObject[] areaCorners = new GameObject[2];

    public int NumberOfFish = 50;
    public GameObject FishPrefab;

    public List<GameObject> fishList;

    private void Awake()
    {
        fishList = new List<GameObject>();
        areaCorners[0] = transform.GetChild(0).gameObject;
        areaCorners[1] = transform.GetChild(1).gameObject;
    }


    public void SpawnFish()
    {
        //IF fish are already spawned randomise location
        if (fishList.Count > 0) RandomiseFishLocation();
        // if there arent enough fish spawned in spawn more
        int fishtoSpawn = NumberOfFish - fishList.Count;
        for (int i = 0; i < fishtoSpawn; i++)
        {
            
            GameObject newFish = Instantiate(FishPrefab,
                new Vector3(Random.Range(areaCorners[0].transform.position.x, areaCorners[1].transform.position.x), Random.Range(areaCorners[0].transform.position.y, areaCorners[1].transform.position.y), 0),
                Quaternion.identity);
            newFish.transform.parent = transform;
            fishList.Add(newFish);
        }
    }

    public void RandomiseFishLocation()
    {
        foreach (GameObject Fish in fishList)
        {
            Fish.SetActive(true);
            Fish.transform.parent = transform;
            Fish.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            Fish.transform.position = new Vector3(Random.Range(areaCorners[0].transform.position.x, areaCorners[1].transform.position.x), Random.Range(areaCorners[0].transform.position.y, areaCorners[1].transform.position.y));
        }
    }

    public void ChangeNumberOfFish(int newFishCount)
    {
        NumberOfFish = newFishCount;
    }

    public void AddToFishCount(int fishIncreaseAmount)
    {
        NumberOfFish = fishIncreaseAmount;
    }



    #if UNITY_EDITOR
    
    private void OnDrawGizmos()
    {
        if (areaCorners[0] != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(areaCorners[0].transform.position, Vector3.one);
            Gizmos.DrawCube(areaCorners[1].transform.position, Vector3.one);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(areaCorners[0].transform.position, new Vector3(areaCorners[1].transform.position.x, areaCorners[0].transform.position.y, 0));
            Gizmos.DrawLine(new Vector3(areaCorners[1].transform.position.x, areaCorners[0].transform.position.y, 0), new Vector3(areaCorners[1].transform.position.x, areaCorners[1].transform.position.y, 0));

            Gizmos.DrawLine(areaCorners[0].transform.position, new Vector3(areaCorners[0].transform.position.x, areaCorners[1].transform.position.y, 0));
            Gizmos.DrawLine(new Vector3(areaCorners[0].transform.position.x, areaCorners[1].transform.position.y, 0), new Vector3(areaCorners[1].transform.position.x, areaCorners[1].transform.position.y, 0));
        }
    }
#endif
}
