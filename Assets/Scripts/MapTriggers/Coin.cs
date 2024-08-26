using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MapTriggerBasicLogic
{
    private Player _player;
    public int price;

    protected override void EnterEvent()
    {
        // GameManager.Instance.dataManager.SaveData.Coin += price;
        GameManager.Instance.dataManager.Coin += price;
        gameObject.SetActive(false);
    }

    protected override void StayEvent()
    {
    }

    protected override void ExitEvent()
    {
    }
}
