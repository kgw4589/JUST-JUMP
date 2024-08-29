using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class FallingBlock : MapTriggerBasicLogic
{
    [Header("TriggerMode 설정 (프리펩은 세팅 해놨으니 수정 X)")]
    public TriggerMode triggerMode;

    [Header("TriggerMode = Falling일 때 수정")]
    [SerializeField] private float moveDis;
    [SerializeField][Range(0.0001f, 0.5f)] private float speed;
    
    [SerializeField] private GameObject effect;
    [Header("TriggerMode = FadeOut일 때 수정")]
    [SerializeField] private float onDelay;
    [SerializeField] private float offDelay;

    
    private float _animSpeed;
    
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
    private void Start()
    {
        _anim = GetComponent<Animator>();
        var position = transform.position;
        _endPos = new Vector2(position.x, position.y - moveDis);
        _startPos = position;
        _animSpeed = 1 / offDelay;
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
                       effect.SetActive(true);
                   }
                   else
                   {
                       _up = true;
                       effect.SetActive(false);
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
                    _anim.speed = _animSpeed;
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

                if (!_col)
                {
                    _anim.speed = 0;
                    if (!_turn)
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
