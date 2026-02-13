using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Trigger2D : MonoBehaviour
{
    public string[] Tags;

    public UnityEvent<GameObject> TriggerEnter;
    public UnityEvent<GameObject> TriggerStay;
    public UnityEvent<GameObject> TriggerExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Tags.Contains(collision.transform.tag) || Tags.Length == 0)
            TriggerEnter.Invoke(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Tags.Contains(collision.transform.tag) || Tags.Length == 0)
            TriggerStay.Invoke(collision.gameObject);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Tags.Contains(collision.transform.tag) || Tags.Length == 0)
            TriggerExit.Invoke(collision.gameObject);

    }
}
