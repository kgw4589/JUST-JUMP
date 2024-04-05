using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTriggerChange : MonoBehaviour
{
    private Player _player;

    [SerializeField] private float bouncePower;
    [SerializeField] private Vector2 bounceDirection;
    
    public enum TriggerState
    {
        Bounce,
    }

    public TriggerState triggerState;
    
    void Start()
    {
        _player = FindObjectOfType<Player>();
    }
    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (triggerState)
        {
            case TriggerState.Bounce:
                Bounce();
                break;
        }
    }

    void Bounce()
    {
        _player.Bounce(bounceDirection, bouncePower);
    }
}
