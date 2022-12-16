using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public GameObject buddy;

    void Update()
    {
        if(buddy == null) return;
        
        if(buddy.transform.position.x >= transform.position.x)
        {
            Vector3 position = transform.position;
            position.x = buddy.transform.position.x;
            transform.position = position;
        }
    }
    
    
}
