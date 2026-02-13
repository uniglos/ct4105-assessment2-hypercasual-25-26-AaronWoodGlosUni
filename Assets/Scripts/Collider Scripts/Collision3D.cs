using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class Collision3D : MonoBehaviour
{
    public UnityEvent<GameObject> CollisionEnter;
    public UnityEvent<GameObject> CollisionStay;
    public UnityEvent<GameObject> CollisionExit;

    public string[] Tags;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {         
        if (Tags.Contains(collision.transform.tag) || Tags.Length == 0)
        {            
            CollisionEnter.Invoke(collision.gameObject);           
        }
    }

    private void OnCollisionStay(UnityEngine.Collision collision)
    {
        if (Tags.Contains(collision.transform.tag) || Tags.Length == 0)
        {
            CollisionStay.Invoke(collision.gameObject);
        }
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (Tags.Contains(collision.transform.tag) || Tags.Length == 0)
        {
            CollisionExit.Invoke(collision.gameObject);
        }
    }
}
