using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyBagsStones : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "buddy") Destroy(gameObject);
    }
}
