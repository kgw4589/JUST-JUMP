using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    
    void Awake()
    {
        var camera = GetComponent<Camera>();
        var r = camera.rect;
        var scaleHeight = ((float)Screen.width / Screen.height) / (9f / 16f);
        var scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1f)
        {
            r.height = scaleHeight;
            r.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            r.width = scaleWidth;
            r.x = (1f - scaleWidth) / 2f;
        }
        
        camera.rect = r;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
    
    void Update()
    {
        var dirPos = new Vector3(0, player.transform.position.y, 0) + offset;
        
        transform.position = Vector3.Lerp(transform.position, dirPos, 1f);
    }
}
