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
    private Vector2 _setPos;
    private bool _onCollider;
    public float maxDis;
    private float _speed;
    
    [SerializeField] private bool destroyTriggerObject;
    [SerializeField] private UnityEvent onTriggerEnterEvent;
    [SerializeField] private UnityEvent onTriggerStayEvent;
    [SerializeField] private UnityEvent onTriggerExitEvent;

    void Start()
    {
        _player = playerObj.GetComponent<Player>();
        _setPos = gameObject.transform.position;
    }

    private void Update()
    {
        float distance = Vector2.Distance(gameObject.transform.position, _setPos);
        if (distance > 0 && gameObject.CompareTag("Elevator") && !_onCollider)
        {
            transform.position = Vector2.MoveTowards(transform.position, _setPos, _speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != playerObj) return;
        
        onTriggerEnterEvent.Invoke();

        _onCollider = true;
        
        if (destroyTriggerObject) Destroy(gameObject);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject != playerObj) return;
        
        onTriggerStayEvent.Invoke();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != playerObj) return;
        
        onTriggerExitEvent.Invoke();

        _onCollider = false;
    }

    public void ChangeSpeed(float speed)
    {
        _player.GetComponent<Player>().ChangeJumpPower(speed);
    }
    
    public void SpringBoard(float power)
    {
        _player.GetComponent<Player>().SpringBoard(power);
    }

    public void ElevatorMove(float speed)
    {
        _speed = speed;
        float distance = Vector2.Distance(gameObject.transform.position, _setPos);
        if(distance < maxDis)
            transform.position += transform.up * speed * Time.deltaTime;
    }
}
