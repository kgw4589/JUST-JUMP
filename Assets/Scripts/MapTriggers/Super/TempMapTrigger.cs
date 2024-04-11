using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TempMapTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnterEvent();
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StayEvent();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ExitEvent();
        }
    }

    public virtual void EnterEvent() { }
    public virtual void StayEvent() { }
    public virtual void ExitEvent() { }
}