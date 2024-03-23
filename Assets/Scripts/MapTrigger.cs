using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MapTrigger : MonoBehaviour
{
    [SerializeField] private GameObject playerObj;
    private Player _player;
    
    [SerializeField] private bool destroyTriggerObject;
    [SerializeField] private UnityEvent onTriggerEnterEvent;
    [SerializeField] private UnityEvent onTriggerExitEvent;

    void Start()
    {
        _player = playerObj.GetComponent<Player>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != playerObj) return;
        
        onTriggerEnterEvent.Invoke();
        
        if (destroyTriggerObject) Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != playerObj) return;
        
        onTriggerExitEvent.Invoke();
    }

    public void ChangeSpeed(float speed)
    {
        // _player.GetComponent<Player>().ChangeJumpPower(speed);
    }
}
