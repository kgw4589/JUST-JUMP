using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MapTriggerBasicLogic
{
    private bool _collision;
    private Vector2 _setPos;

    [SerializeField] private float maxDis;
    [SerializeField] private float speed;

    private void Start()
    {
        _setPos = new Vector2(transform.position.x, transform.position.y);
        Debug.Log(_setPos);
    }

    private void Update()
    {
        
        if (!_collision)
        {
            transform.position = Vector2.Lerp(gameObject.transform.position, _setPos, 0.05f);

        }
    }

    protected override void EnterEvent()
    {
        
    }

    protected override void StayEvent()
    {
        _collision = true;
        float distance = Vector2.Distance(
            gameObject.transform.position, _setPos);
        if(distance < maxDis)
            transform.position += transform.up * speed * Time.deltaTime;
    }

    protected override void ExitEvent()
    {
        _collision = false;
    }
}
