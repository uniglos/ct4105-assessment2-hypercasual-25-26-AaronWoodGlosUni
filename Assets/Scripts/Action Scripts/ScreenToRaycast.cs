using UnityEngine;
using UnityEngine.Events;

public class ScreenToRaycast : MonoBehaviour
{
    Camera cam;
    public float RayCastDistance = float.MaxValue;
    public LayerMask LayerMask;

    [SerializeField]
    UnityEvent<Vector3> RayCast3dComplete;
    [SerializeField]
    UnityEvent<Vector2> RayCast2dComplete;

    private void Start()
    {
        cam = Camera.main;
    }

   
    public void RayCast3D(Vector2 Test)
    {    
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(Test.x,Test.y,cam.nearClipPlane));        
        
        RaycastHit hit;
        Debug.DrawRay(point, cam.transform.TransformDirection(Vector3.forward) * 10, UnityEngine.Color.red);
        if (Physics.Raycast(point,cam.transform.TransformDirection(Vector3.forward), out hit, RayCastDistance,LayerMask)) {
            RayCast2dComplete.Invoke(hit.point);
            if (hit.transform.GetComponent<InteractiveObject>() != null) {
                hit.transform.GetComponent<InteractiveObject>().Interacted(hit.point);
            }
        }       
    }
    public void RayCast2D(Vector2 screenPos) {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(screenPos), Vector2.zero);

        if (hit.collider != null) {
            print(hit.collider.name);
            RayCast2dComplete.Invoke(screenPos);
        }
    }
}
