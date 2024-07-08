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

    private Animator _anim;
    
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
    private static readonly int BreakSpeed = Animator.StringToHash("breakSpeed");

    private void Start()
    {
        var position = transform.position;
        _endPos = new Vector2(position.x, position.y - moveDis);
        _startPos = position;
        _anim.SetFloat(BreakSpeed, 0);
    }

    private void Update()
    {
        switch (triggerMode)
        {
            case TriggerMode.Falling:
               float distance = Vector2.Distance(transform.position, _endPos);
               if (_up)
               {
                   transform.position = Vector2.MoveTowards(transform.position, _startPos, speed * 5);
                   
                   if (distance >= moveDis && _col)
                   {
                       _up = false;
                   }
               }
               else
               {
                   if (_col)
                   {
                       _up = false;
                   }

                   transform.position = Vector2.MoveTowards(transform.position, _endPos, speed);
                   float distance1 = Vector2.Distance(gameObject.transform.position, _startPos);
                   if (distance1 >= moveDis)
                   {
                       _up = true;
                   }
               }
               break;
            case TriggerMode.FadeOut:
                if (_col && _turn)
                {
                    _anim.SetFloat("breakSpeed", speed); 
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
                        _anim.Play("breakSpeed", -1, 0);
                        var position = transform.position;
                        position = new Vector2(position.x - 20, position.y);
                        transform.position = position;
                        _anim.SetFloat("breakSpeed", 0);
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
