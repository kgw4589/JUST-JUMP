using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TempMapTrigger : MonoBehaviour
{
    [SerializeField] protected string triggerTagName = "Player"; 
    
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

    public virtual void EnterEvent() { }
    public virtual void StayEvent() { }
    public virtual void ExitEvent() { }
}