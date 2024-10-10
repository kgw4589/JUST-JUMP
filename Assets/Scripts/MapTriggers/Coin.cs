using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MapTriggerBasicLogic
{
    private Player _player;
    private const int _PRICE = 1;

    private void Update()
    {
        transform.Rotate(0, 5, 0);
    }

    protected override void EnterEvent()
    {
        SoundManager.Instance.PlaySfx(SoundManager.Sfx.coin);
        DataManager.Instance.SaveData.coin += _PRICE * MapManager.Instance.selectedMapScriptable.coinWeight;
        UIManager.Instance.SetCoinUI(DataManager.Instance.SaveData.coin);
        gameObject.SetActive(false);
    }

    protected override void StayEvent()
    {
    }

    protected override void ExitEvent()
    {
    }
}
