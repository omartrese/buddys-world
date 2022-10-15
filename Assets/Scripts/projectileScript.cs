using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    private enemyScript enemy;
    private buddyMovement buddy;

    private void Start()
    {
        enemy = GetComponent<enemyScript>();
        buddy = GetComponent<buddyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Destroy(gameObject);

        if(other.gameObject.tag == "Player")
        {
            buddy.hit();
        } else if(other.gameObject.tag == "enemy")
        {
            enemy.hit();
        }
    }
}
