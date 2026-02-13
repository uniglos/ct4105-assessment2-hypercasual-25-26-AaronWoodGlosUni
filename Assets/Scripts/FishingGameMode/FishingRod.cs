using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class FishingRod : MonoBehaviour
{
    public GameObject Bait;
    LineRenderer lineRenderer;
    public Vector3 rodOffset = Vector3.zero;
    public Vector3 baitOffset = Vector3.zero;
    public UnityEvent gameStart;

    MoveObject baitMoveObjectScript;

    [Header("GamePlay Variables")]
    [Space]
    public float RodSpead = 1f;
    public float FishingLineLength = 1;
    public float baitSize = 1;
    public int BaitBaseDepth = 0;
    Vector3 baitBaseDepth;
    public Vector3 baitVec3Depth = Vector3.zero;


    private void OnValidate()
    {
        baitBaseDepth =  Bait.transform.position + (Vector3.up * -BaitBaseDepth);
    }

    public float _baitSize { get { return baitSize; } set {  baitSize = value; setBaitSize(); } }
    public float _FishingLineLength { get { return FishingLineLength; } set { FishingLineLength = value; setBaitDepth(); } }

    void setBaitDepth()
    {
        baitVec3Depth = Vector3.up * FishingLineLength;
    }
    void setBaitSize()
    {
        Bait.transform.localScale = Vector3.one * baitSize;
    }



    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();       
        baitMoveObjectScript = Bait.GetComponent<MoveObject>();
        _FishingLineLength = FishingLineLength;
        baitBaseDepth = Bait.transform.position + (Vector3.up * -BaitBaseDepth);
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position+rodOffset);
        lineRenderer.SetPosition(1,Bait.transform.position + baitOffset);
    }

    public void ReelInBait()
    {

    }

    public void GoFish()
    {
        
        baitMoveObjectScript.ChangeEndPosition(baitBaseDepth - baitVec3Depth);
        baitMoveObjectScript.StartMoving(RodSpead); 
        gameStart.Invoke();
        
    }

    

}
