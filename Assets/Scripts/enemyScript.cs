using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public GameObject buddy;

    // Update is called once per frame
    void Update()
    {
        if((buddy.transform.position.x - transform.position.x) < 0)
        {
            transform.localScale = new Vector3(-0.3f, 0.3f, 0.3f);
        } else transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
    }
}
