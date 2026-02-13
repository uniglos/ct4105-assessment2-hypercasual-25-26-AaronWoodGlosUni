using UnityEngine;
using UnityEngine.Events;

public class AppendString : MonoBehaviour
{
    [SerializeField] UnityEvent<string> appendStringEvent;
    [SerializeField] string appendBefore="";
    [SerializeField] string appendAfter="";

    public void AppendInputString(string input) {
        appendStringEvent.Invoke(appendBefore + input + appendAfter);
    }
    public void AppendInputString(float input) {
        appendStringEvent.Invoke(appendBefore + input.ToString() + appendAfter);
    }
    public void AppendInputString(int input) {
        appendStringEvent.Invoke(appendBefore + input.ToString() + appendAfter);
    }
    public void AppendInputString(Vector2 input) {
        appendStringEvent.Invoke(appendBefore + input.ToString() + appendAfter);
    }
    public void AppendInputString(Vector3 input) {
        appendStringEvent.Invoke(appendBefore + input.ToString() + appendAfter);
    }
}