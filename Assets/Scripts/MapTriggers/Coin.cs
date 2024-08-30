using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MapTriggerBasicLogic
{
    private Player _player;
    private const int _PRICE = 1;

    protected override void EnterEvent()
    {
        GameManager.Instance.datamanager.SaveData.coin += _PRICE * MapManager.Instance.selectedMapScriptable.coinWeight;
        gameObject.SetActive(false);
    }

    protected override void StayEvent()
    {
    }

    protected override void ExitEvent()
    {
    }
}
