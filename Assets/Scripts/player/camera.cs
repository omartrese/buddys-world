using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject buddy;

    void Update()
    {
        if(buddy == null) return;

        Vector3 position = transform.position;
        position.x = buddy.transform.position.x;
        transform.position = position;

    }
}
