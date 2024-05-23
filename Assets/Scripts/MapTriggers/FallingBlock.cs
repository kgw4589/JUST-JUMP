using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingBlock : MapTriggerBasicLogic
{
    [SerializeField] private float moveDis;
    [SerializeField][Range(0.0001f, 0.5f)] private float speed;
    
    [SerializeField] private float onDelay;
    [SerializeField] private float offDelay;
    
    private Vector2 _startPos;
    private Vector2 _endPos;
    
    private bool _up = true;
    private bool _turn = true;
    private float _currentTime = 0;
    
    private bool _col;
    
    public enum TriggerMode
    {
        Falling,
        FadeOut
    }

    public TriggerMode triggerMode;
    private void Start()
    {
        var position = transform.position;
        _endPos = new Vector2(position.x, position.y - moveDis);
        _startPos = position;
    }

    private void Update()
    {
        switch (triggerMode)
        {
            case TriggerMode.Falling:
               float distance = Vector2.Distance(transform.position, _startPos);
               if (_up)
               {
                   transform.position = Vector2.MoveTowards(transform.position, _startPos, speed * 2);
                   if (distance == 0 && _col)
                   {
                       _up = !_up;
                   }
               }
               else if (!_up)
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
               break;
            case TriggerMode.FadeOut:
                if (_col && _turn)
                {
                    _currentTime += Time.deltaTime;
                    if (_currentTime > offDelay)
                    {
                        var position = transform.position;
                        position = new Vector2(position.x + 20, position.y);
                        transform.position = position;
                        _currentTime = 0;
                        _turn = false;
                    }
                }

                if (!_col && !_turn)
                {
                    _currentTime += Time.deltaTime;
                    if (_currentTime > onDelay)
                    {
                        var position = transform.position;
                        position = new Vector2(position.x - 20, position.y);
                        transform.position = position;
                        _currentTime = 0;
                        _turn = true;
                    }
                }
                break;
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
