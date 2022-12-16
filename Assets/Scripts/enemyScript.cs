using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public GameObject buddy;
    public GameObject enemy;
    public float enemyRayLength;
    public GameObject dirt;
    float objectsDistance;  
    public buddyMovement buddyScript;

    private bool canShootEnemy;
    public AudioClip enemyThrowSound;
    
    private Vector2 enemyDirection;
    public Transform shootOrigin;
    public float dirtSpeed;
    public float dirtCooldown;
    public int health = 3;

    private AudioSource enemyAudioSource;

    void Start()
    {
        enemyAudioSource = GetComponent<AudioSource>();    
        canShootEnemy = true;
        if(health <= 0)
        {
            health = 1;
        }
        Debug.Log("Enemy's health is: " + health);
    }

    void Update()
    {
        if(enemy == null) return;
        if(buddy == null) return;

        if((buddy.transform.position.x - transform.position.x) < 0)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
        } else 
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

        objectsDistance = Mathf.Abs(buddy.transform.position.x - transform.position.x);

       

        
    }

    public IEnumerator Shoot()
    {
        
        int enemyShootDirection()
        {
            if(transform.localScale.x < 0f)
            {
                return -1;
            } 
            else if(transform.localScale.x > 0f)
            {
                return +1;
            }

            return 0;
        }
        if(buddyScript.playerHealth > 0)
        {

            enemyAudioSource.PlayOneShot(enemyThrowSound);  
            canShootEnemy = false;
            GameObject instantiatedDirt = Instantiate(dirt, shootOrigin.position, Quaternion.identity);
            instantiatedDirt.GetComponent<Rigidbody2D>().velocity = new Vector2(dirtSpeed * enemyShootDirection() * Time.deltaTime, 0f);
    
            yield return new WaitForSeconds(dirtCooldown);
            Destroy(instantiatedDirt);
            canShootEnemy = true;   
        }
    }

    private void FixedUpdate()
    {
       if(canShootEnemy && objectsDistance < 9.0f)
       {
        StartCoroutine(Shoot());
       }    
    }

    public void hit()
    {
        if(health <= 0) 
        {
            Destroy(gameObject);
        }
        else
        {
            health--;
            Debug.Log("enemy health: " + health);
            if(health == 0) Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "projectile")
        {
            hit();
        }
    }
}
