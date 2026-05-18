using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damage : MonoBehaviour
{
    private bool entered = false;
    
    public bool destroyEnemy = false;

    public float delayWhilstHit = 0.5f;

    public float knockUnits = 1.0f;

    public float knockPowerX = 0.5f;

    public float knockPowerY = 0.1f;

    public AudioSource damageAudio;

    public UnityEvent doDecreaseLife;

    void Start() {
  
    }

    void OnTriggerEnter(Collider collision)
    {

        if( !entered )
        {

            entered = true;

            if( HealthManagerScript.lives > 0 )
            {

                doDecreaseLife.Invoke();

                if( destroyEnemy )
                {

                    Destroy( gameObject );

                }

                if( damageAudio )
                {

                    AudioSource.PlayClipAtPoint( damageAudio.clip, transform.position );

                }

            }

        }

        if( collision.gameObject.tag == "Player" )
        {

            StartCoroutine( Knockback( knockUnits, knockPowerX, knockPowerY, collision.gameObject ) );

        }

    }

    void OnTriggerExit(Collider collision)
    {

        entered = false;
        
    }

    void OnCollisionEnter(Collision collision)
    {

        if( !entered )
        {

            entered = true;

            if( HealthManagerScript.lives > 0 )
            {

                doDecreaseLife.Invoke();

                if( destroyEnemy )
                {

                    Destroy( gameObject );

                }

                if( damageAudio )
                {
                    
                    AudioSource.PlayClipAtPoint( damageAudio.clip, transform.position );

                }

            }

        }

        if( collision.gameObject.tag == "Player" )
        {

            StartCoroutine( Knockback( knockUnits, knockPowerX, knockPowerY, collision.gameObject ) );

        }

    }

    void OnCollisionExit(Collision collision)
    {

        entered = false;
        
    }

    public IEnumerator Knockback( float knockUnits, float knockPowerX, float knockPowerY, GameObject collision )
    {
    
        float timer = 0;
    
        while( ( ( knockUnits * 10 ) / 2 ) > timer ) 
        {

            timer += Time.deltaTime;

            Vector2 direction = new Vector2( this.transform.position.x - collision.transform.position.x, collision.transform.position.y * ( knockPowerY / 100 ) ).normalized;

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce( -direction * ( knockPowerX * 10 ) );

            collision.gameObject.GetComponent<SwervePlayerController>().enabled = false;

            collision.gameObject.GetComponent<Animator>().SetBool( "grounded", true );


        }

        yield return new WaitForSeconds( delayWhilstHit );

        collision.gameObject.GetComponent<SwervePlayerController>().enabled = true;
                                                                                                                                                                                                                                                                                                                                                                                                                                        
    } 

}