using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyProjectiles : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "buddy" || other.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag != "stonesBag")
        {
            Destroy(gameObject);
        }

        if(other.gameObject.tag == "buddy" || other.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
        }
    }
}
