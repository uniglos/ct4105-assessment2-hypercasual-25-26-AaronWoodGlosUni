using UnityEngine;
using UnityEngine.Events;

public class RandomVector3Gen : MonoBehaviour
{
    [SerializeField] float xLowerBounds;
    [SerializeField] float xUpperBounds;
    [Space]
    [SerializeField] float yLowerBounds;
    [SerializeField] float yUpperBounds;
    [Space]
    [SerializeField] float zLowerBounds;
    [SerializeField] float zUpperBounds;
    [Space]
    [SerializeField] UnityEvent<Vector3> onGetVector3;

    public void GetRandomVector3() {
        onGetVector3.Invoke(new Vector3(Random.Range(xLowerBounds,xUpperBounds), Random.Range(yLowerBounds, yUpperBounds), Random.Range(zLowerBounds, zUpperBounds)));
    }
}