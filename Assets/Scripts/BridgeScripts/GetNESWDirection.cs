using UnityEngine;
using UnityEngine.Events;

public class GetNESWDirection : MonoBehaviour
{
    [SerializeField] UnityEvent<Vector2> OnFoundDirection;
    float x;
    float y;

    public void FindDirection90Deg(Vector2 direction) {
        //Gets the Absolute Value of X and Y
       x = direction.x>=0 ? direction.x : -direction.x;
       y = direction.y>=0 ? direction.y : -direction.y;        
        if (x > y) {
            OnFoundDirection.Invoke(new Vector2(direction.x,0));            
        }
        else {            
            OnFoundDirection.Invoke(new Vector2(0, direction.y));
        }
    }


    public void FindDirectionNormal(Vector2 direction)
    {
        //Gets the Absolute Value of X and Y
        x = direction.x >= 0 ? direction.x : -direction.x;
        y = direction.y >= 0 ? direction.y : -direction.y;
        if (x > y)
        {
            if (direction.x < 0)
            {
                OnFoundDirection.Invoke(Vector2.left);
            }
            else
            {
                OnFoundDirection.Invoke(Vector2.right);
            }
        }
        else
        {
            if (direction.x < 0)
            {
                OnFoundDirection.Invoke(Vector2.up);
            }
            else
            {
                OnFoundDirection.Invoke(Vector2.down);
            }
        }
    }



    public void FindLeftRight(Vector2 direction) {
        if (direction.x > 0)
        {
            OnFoundDirection.Invoke(Vector2.right);
            
        }
        else {
            OnFoundDirection.Invoke(Vector2.left);
            
        }
            
        
    }

    
}