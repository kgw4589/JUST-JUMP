using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlock : MapTriggerBasicLogic
{
    [SerializeField] private float moveDis;

    [SerializeField][Range(0.0001f, 0.5f)] private float speed;
    private Vector2 _startPos;
    private Vector2 _endPos;
    private bool _up = true;
    private bool _col;  
    private void OnEnable()
    {
        _endPos = new Vector2(transform.position.x, transform.position.y - moveDis);
        _startPos = transform.position;
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, _startPos);
        if (_up)
        {
            transform.position = Vector2.MoveTowards(transform.position, _startPos, speed * 2);
            if (distance == 0 && _col)
            {
                _up = !_up;
            }
        }
        else if(!_up)
        {
            if (_col)
            {
                _up = false;
            }
            transform.position = Vector2.MoveTowards(transform.position, _endPos, speed);
            float distance1 = Vector2.Distance(gameObject.transform.position, _endPos);
            if (distance1 == 0)
            {
                _up = !_up;
            }
        }
    }

    protected override void EnterEvent()
    {
        _col = true;
    }

    protected override void StayEvent()
    {
    }

    protected override void ExitEvent()
    {
        _col = false;
    }
}
