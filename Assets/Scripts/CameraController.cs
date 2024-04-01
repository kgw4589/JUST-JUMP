using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var dirPos = new Vector3(0, player.transform.position.y, 0) + offset;
        
        transform.position = Vector3.Lerp(transform.position, dirPos, 1f);
    }
}
