using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : TempMapTrigger
{
    private Player _player;

    [SerializeField] private float speed;
    public override void StayEvent()
    {
        _player.GetComponent<Player>().ChangeJumpPower(speed);
    }
}
