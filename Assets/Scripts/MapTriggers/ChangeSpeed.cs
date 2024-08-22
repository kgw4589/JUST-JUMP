using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MapTriggerBasicLogic
{
    private Player _player;

    [SerializeField] private float speed;
    [SerializeField] private GameObject effect;

    private void OnEnable()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    protected override void EnterEvent()
    {
    }

    protected override void StayEvent()
    {
        effect.transform.position = _player.transform.position;
        effect.SetActive(true);
        _player.GetComponent<Player>().ChangeJumpPower(speed);
    }

    protected override void ExitEvent()
    {
        effect.transform.position = gameObject.transform.position;
        effect.SetActive(false);
    }
}
