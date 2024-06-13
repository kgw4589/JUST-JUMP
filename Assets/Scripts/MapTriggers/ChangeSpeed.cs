using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MapTriggerBasicLogic
{
    private Player _player;

    [SerializeField] private float speed;

    private void OnEnable()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    protected override void EnterEvent()
    {
        
    }

    protected override void StayEvent()
    {
        _player.GetComponent<Player>().ChangeJumpPower(speed);
    }

    protected override void ExitEvent()
    {
        
    }
}
