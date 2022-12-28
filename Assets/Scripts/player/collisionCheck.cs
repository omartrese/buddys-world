using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionCheck : MonoBehaviour
{
    public buddyMovement buddy;


    private void OnCollisionEnter2D(Collision2D other)
    {
        buddy.canMove = false;
    }
}
