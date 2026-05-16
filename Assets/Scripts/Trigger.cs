using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    [SerializeField] public GameObject gameObjectToTrigger;
    [SerializeField] public string triggerAnimatorParamater;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    private void OnTriggerEnter( Collider collision )
    {
        
        if( collision.gameObject.tag == "Player")
        {
            if( gameObjectToTrigger )
            {

                gameObjectToTrigger.GetComponent<Animator>().SetTrigger( triggerAnimatorParamater );

            }

        }      

    }

    private void OnTriggerStay( Collider collision )
    {
    
        if( collision.gameObject.tag == "Player")
        {

            if( gameObjectToTrigger )
            {
                gameObjectToTrigger.GetComponent<Animator>().SetTrigger( triggerAnimatorParamater );

            }

        }

    }

    private void OnTriggerExit( Collider collision )
    {
        if( collision.gameObject.tag == "Player")
        {
            if( gameObjectToTrigger )
            {

                gameObjectToTrigger.GetComponent<Animator>().ResetTrigger( triggerAnimatorParamater ); 

            }

        }  

    }

     private void OnCollisionEnter( Collision collision )
    {
        if( collision.gameObject.tag == "Player")
        {

            if( gameObjectToTrigger )
            {

                gameObjectToTrigger.GetComponent<Animator>().SetTrigger( triggerAnimatorParamater );
                
            }

        }

    }

    private void OnCollisionStay( Collision collision )
    {
        if( collision.gameObject.tag == "Player")
        {
            if( gameObjectToTrigger )
            {

                gameObjectToTrigger.GetComponent<Animator>().SetTrigger( triggerAnimatorParamater );

            }

        }

    }

    private void OnCollisionExit( Collision collision )
    {
                      
        if( collision.gameObject.tag == "Player")
        {

            if( gameObjectToTrigger )
            {

                gameObjectToTrigger.GetComponent<Animator>().ResetTrigger( triggerAnimatorParamater ); 

            }

        }   

    }

}
