using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : TempMapTrigger
{
    private Player _player;

    [SerializeField] private float bouncePower;
    [SerializeField] private Vector2 bounceDirection;

    public override void EnterEvent()
    {
        _player.Bounce(bounceDirection, bouncePower);
    }
}
