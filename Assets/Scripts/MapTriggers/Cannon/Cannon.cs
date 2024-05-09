using System;
using System.Collections;
using System.Collections.Generic;
using MapTriggers.Cannon;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private float _distance;
    private float _bulletDistance;
    private float _currentTime;
    private bool _reload;

    [SerializeField] private float shootingTime = 5f;
    [SerializeField] private float pushForce = 3f;
    
    [SerializeField] private CannonBullet cannonBullet;
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private Vector2 endPoint;
    
    private Vector2 _startPoint;
    private Vector2 _direction;
    private Vector2 _force;

    private void Awake()
    {
        _startPoint = transform.position;
        endPoint = new Vector2(endPoint.x + _startPoint.x, endPoint.y + _startPoint.y);
    }

    void Update()
    {
        _distance = Vector2.Distance(_startPoint, endPoint);
        _direction = (_startPoint - endPoint).normalized;
        _force = _direction * (_distance * pushForce);
        
        Debug.DrawLine(_startPoint,endPoint);
        
        _currentTime += Time.deltaTime;
        _bulletDistance = Vector2.Distance(transform.position, bulletObj.transform.position);

        
        if (_bulletDistance > 10f)
        {
            bulletObj.SetActive(false);
            cannonBullet.rb.gravityScale = 0f;
            bulletObj.transform.position = transform.position;
            _reload = true;
            _currentTime = 0;
        }

        if (_currentTime > shootingTime && _reload)
        {
            Shooting();
            _currentTime = 0;
        }
        else if (_currentTime > shootingTime - 0.75f && !_reload)
        {
            cannonBullet.rb.gravityScale = 0f;

            bulletObj.SetActive(true);
        
            Debug.DrawLine(_startPoint,endPoint);
        
            trajectory.UpdateDots(cannonBullet.Pos,_force);
            
            trajectory.Show();

            _reload = true;
        }
        
    }

    void Shooting()
    {
        cannonBullet.Push(_force);
        _reload = false;
        trajectory.Hide();
    }
}
