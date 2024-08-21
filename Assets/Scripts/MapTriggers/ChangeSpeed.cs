using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpeed : MapTriggerBasicLogic
{
    private Player _player;
    private bool _col;

    [SerializeField] private float speed;
    [SerializeField] private GameObject effect;

    private void OnEnable()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        PlayEffect();
    }

    protected override void EnterEvent()
    {
        _col = true;
    }

    protected override void StayEvent()
    {
        _player.GetComponent<Player>().ChangeJumpPower(speed);
    }

    protected override void ExitEvent()
    {
        _col = false;
    }

    protected override void PlayEffect()
    {
        // _currentTime += Time.deltaTime;
        // if (_currentTime > effectTime)
        // {
        //     
        // }
        if (_col)
        {
            effect.transform.position = _player.transform.position;
            effect.SetActive(true);
        }
        else
        {
            effect.transform.position = gameObject.transform.position;
            effect.SetActive(false);
        }
    }
}
