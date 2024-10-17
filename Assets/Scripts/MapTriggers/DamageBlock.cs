using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBlock : MapTriggerBasicLogic
{
    [SerializeField] private GameObject effect;
    [SerializeField] private float hitTime = 0.5f;

    [SerializeField] private float damage = 0.75f;
    
    private float _currentTime;
    private Player _player;
    private bool _col;
    private void OnEnable()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
        if(_col)
        {
            effect.transform.position = _player.transform.position;
            _currentTime += Time.deltaTime;

            if (!(_currentTime > hitTime)) return;
            _player.GetDamage(damage);
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.tickDamage);
            _currentTime = 0;
        }
    }

    protected override void EnterEvent()
    {
        effect.SetActive(true);
        _col = true;
    }

    protected override void StayEvent()
    {
    }

    protected override void ExitEvent()
    {
        effect.transform.position = gameObject.transform.position;
        effect.SetActive(false);
        _col = false;
    }
}
