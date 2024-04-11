using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : TempMapTrigger
{
    private bool _collision;
    private Vector2 _setPos;

    [SerializeField] private float maxDis;
    [SerializeField] private float speed;

    private void Awake()
    {
        _setPos = gameObject.transform.position;
    }

    private void Update()
    {
        if (!_collision)
        {
            transform.position = Vector2.Lerp(gameObject.transform.position, _setPos, 0.05f);
        }
    }

    public override void StayEvent()
    {
        _collision = true;
        float distance = Vector2.Distance(
            gameObject.transform.position, _setPos);
        if(distance < maxDis)
            transform.position += transform.up * speed * Time.deltaTime;
    }

    public override void ExitEvent()
    {
        _collision = false;
    }
}
