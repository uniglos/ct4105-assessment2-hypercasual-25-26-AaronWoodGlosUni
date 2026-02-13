using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class ReelInBaitScript : MonoBehaviour
{
    bool reeling = false;
    public FishingRod Rod;
    public UnityEvent ReelInComplete;
    // Start is called before the first frame update
    float startingYpos;
    Vector3 dragOffset = Vector3.zero;

    private void Start()
    {
        startingYpos = transform.position.y;
    }
    public void StartReeling()
    {
        
        reeling = true;
        
    }
     public void HideCaughtFish()
    {
        Transform[] childs = GetComponentsInChildren<Transform>();
        foreach (Transform child in childs)
        {
            if (child == null || child == this.transform || child.tag != "Fish") continue;
            child.transform.parent = null;
            child.gameObject.SetActive(false);
        }
    }

    public void DisableCaughtFishCollider(GameObject fish)
    {
       fish.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void MoveBait(Vector2 Pos)
    {        
        dragOffset = new Vector3(Pos.x * 0.1f,0,0);       
    }

    // Update is called once per frame
    void Update()
    {
        if (reeling) {
            transform.position = transform.position + dragOffset + (Vector3.up * Time.deltaTime * Rod.RodSpead);
            Vector3 tmpPos = transform.position;
            tmpPos.x = Mathf.Clamp(tmpPos.x, -4.0f, 5.0f);
            transform.position = tmpPos;
            if (transform.position.y >= startingYpos)
            {
                reeling = false;
                transform.position = (transform.position - (Vector3.up * transform.position.y))+ Vector3.up * startingYpos;
                ReelInComplete.Invoke();
            }
        }
    }
}
