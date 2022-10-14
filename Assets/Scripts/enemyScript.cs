using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public GameObject buddy;
    
    public float enemyRayLength;
    public GameObject dirt;
    
    private bool canShootEnemy;
    public AudioClip enemyThrowSound;
    
    private Vector2 enemyDirection;
    public Transform shootOrigin;
    public float dirtSpeed;
    public float dirtCooldown;

    private AudioSource enemyAudioSource;

    void Start()
    {
        enemyAudioSource = GetComponent<AudioSource>();    
        canShootEnemy = true;
    }

    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyDirection, enemyRayLength);

        if((buddy.transform.position.x - transform.position.x) < 0)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
            enemyDirection = Vector2.left;
        } else 
        {
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            enemyDirection = Vector2.right;
        }

        Debug.DrawRay(transform.position, enemyDirection * enemyRayLength, Color.yellow);

        if(hit.collider.tag == "Player" && canShootEnemy)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
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
        enemyAudioSource.PlayOneShot(enemyThrowSound);  
        canShootEnemy = false;
        GameObject instantiatedDirt = Instantiate(dirt, shootOrigin.position, Quaternion.identity);
        instantiatedDirt.GetComponent<Rigidbody2D>().velocity = new Vector2(dirtSpeed * enemyShootDirection() * Time.deltaTime, 0f);

        yield return new WaitForSeconds(dirtCooldown);
        Destroy(instantiatedDirt);
        canShootEnemy = true;   
    }
}
