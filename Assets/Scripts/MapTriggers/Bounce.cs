using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MapTriggerBasicLogic
{
    private Player _player;

    [SerializeField] private float bouncePower;
    [SerializeField] private Vector2 bounceDirection;

    protected override void EnterEvent()
    {
        _player.Bounce(bounceDirection, bouncePower);
    }

    protected override void StayEvent()
    {
        throw new NotImplementedException();
    }

    protected override void ExitEvent()
    {
        throw new NotImplementedException();
    }
}
