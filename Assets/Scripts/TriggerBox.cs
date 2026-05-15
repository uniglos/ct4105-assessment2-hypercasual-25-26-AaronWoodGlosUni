using System.Collections;
using UnityEngine;

public class TriggerBox : MonoBehaviour {

    public GameObject uiObject;
    void Start()
    {
        uiObject.SetActive(false);
    }
    // Update is called once per frame
    void OnTriggerEnter (Collider player)
    {
        if(player.gameObject.tag == "Player")
        {
            uiObject.SetActive(true);
            StartCoroutine("WaitForSec");
        }
    }
    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        Destroy(uiObject);
        Destroy(gameObject);
    }
}