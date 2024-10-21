using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkZone : MonoBehaviour
{
    // Start is called before the first frame update
    private Player _player;
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _player.DieZone();
        }        
    }
}
