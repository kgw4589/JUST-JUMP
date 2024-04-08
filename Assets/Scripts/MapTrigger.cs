using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    private Player _player;
    private Vector2 _setPos;

    [SerializeField] private float maxDis;
    [SerializeField] private float speed;
    [SerializeField] private float bouncePower;
    [SerializeField] private Vector2 bounceDirection;
    
    public enum TriggerState
    {
        Bounce,
        ChangeSpeed,
        ElevatorMove
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
            case TriggerState.ChangeSpeed:
                ChangeSpeed();
                break;
            case TriggerState.ElevatorMove:
                ElevatorMove();
                break;
        }
    }

    void Bounce()
    {
        _player.Bounce(bounceDirection, bouncePower);
    }
    void ChangeSpeed()
    {
        _player.GetComponent<Player>().ChangeJumpPower(speed);
    }
    void ElevatorMove()
    {
        float distance = Vector2.Distance(gameObject.transform.position, _setPos);
        if(distance < maxDis)
            transform.position += transform.up * speed * Time.deltaTime;
    }
}
