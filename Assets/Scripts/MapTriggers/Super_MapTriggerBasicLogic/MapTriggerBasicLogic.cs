using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MapTriggerBasicLogic : MonoBehaviour
{
    [SerializeField] protected string triggerTagName = "Player"; 

    protected abstract void EnterEvent();
    protected abstract void StayEvent();
    protected abstract void ExitEvent();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTagName))
        {
            EnterEvent();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(triggerTagName))
        {
            StayEvent();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(triggerTagName))
        {
            ExitEvent();
        }
    }
}