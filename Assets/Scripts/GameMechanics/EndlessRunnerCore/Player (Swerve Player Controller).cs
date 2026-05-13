using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SwervePlayerController : MonoBehaviour
{
    public float moveDistance = 1;
    public float maxSwerveAmount = 1;
    Rigidbody rb;
    Vector2 currentSwerve = Vector2.zero;
    bool isGrounded;
    public float JumpPower= 1;
    public bool AllowJumping = true;
    public float groundedOffset = -0.14f;
    public LayerMask GroundLayerMask;
    float floordistance = 0;    
    Vector3 SpherePos = Vector3.zero;
    

    private void Awake(){
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.Log(isGrounded);
    }

    private void FixedUpdate(){
        SpherePos = new Vector3(transform.position.x,transform.position.y + groundedOffset,transform.position.z);
        isGrounded = Physics.CheckSphere(SpherePos, 0.5f, GroundLayerMask);        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(SpherePos, 0.5f);
    }

    public void SwervePlayer(Vector2 Dir){        
        if (Dir.x != 0)
        {            
            if (currentSwerve.x + Dir.x <= Vector2.right.x * maxSwerveAmount && currentSwerve.x + Dir.x >= Vector2.left.x * maxSwerveAmount)
            {                
                transform.position = new Vector3(transform.position.x + (Dir.x * moveDistance), transform.position.y, transform.position.z);
                currentSwerve += Dir;                
            }
        }
        else{
            
            if (AllowJumping && isGrounded)
            {               
                rb.AddForce(Vector3.up*JumpPower, ForceMode.VelocityChange);
            }
        }
        
    }
}
