using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MapTriggerBasicLogic
{
    private Player _player;

    [SerializeField] private float bouncePower;
    [SerializeField] private Vector2 bounceDirection;

    private void Awake()
    {
        _player = GameObject.FindWithTag(triggerTagName).GetComponent<Player>();
    }

    protected override void EnterEvent()
    {
        _player.Bounce(bounceDirection, bouncePower);
    }

    protected override void StayEvent()
    {
    }

    protected override void ExitEvent()
    {
    }
}
