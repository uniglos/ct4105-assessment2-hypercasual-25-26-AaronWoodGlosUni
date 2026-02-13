using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Collisions2D : MonoBehaviour {

    [SerializeField] private LayerMask MaskTrigger;
    [SerializeField] private string[] Tags;

    [SerializeField] private UnityEvent<GameObject> CollisionEnter;
    [SerializeField] private UnityEvent<GameObject> CollisionStay;
    [SerializeField] private UnityEvent<GameObject> CollisionExit;

    public void OnCollisionEnter2D(Collision2D other) {        
        if ( CheckLayer(other.gameObject.layer) && Tags.Contains(other.transform.tag) || Tags.Length == 0)
        {
            CollisionEnter.Invoke(other.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (CheckLayer(other.gameObject.layer) && (Tags.Contains(other.transform.tag) || Tags == null))
            CollisionStay.Invoke(other.gameObject);        
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (CheckLayer(other.gameObject.layer) && (Tags.Contains(other.transform.tag) || Tags == null))
            CollisionExit.Invoke(other.gameObject);        
    }

    private bool CheckLayer(int layer) {
        if (MaskTrigger.value == 0) return true;
        int bitShift = 1 << layer;
        if ((MaskTrigger & bitShift) != 0) return true;
        else return false;
    }
}
