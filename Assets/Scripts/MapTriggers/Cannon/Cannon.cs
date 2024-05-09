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

        StartCoroutine(Reload());
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        
        _bulletDistance = Vector2.Distance(transform.position, bulletObj.transform.position);
        
        if (_bulletDistance > 10f)
        {
            bulletObj.SetActive(false);
            bulletObj.transform.position = transform.position;
            _currentTime = 0;
        }
    }

    private IEnumerator Reload()
    {
        while (true)
        {
            _distance = Vector2.Distance(_startPoint, endPoint);
            _direction = (_startPoint - endPoint).normalized;
            _force = _direction * (_distance * pushForce);
                    
            Debug.DrawLine(_startPoint,endPoint);
                    
            yield return new WaitUntil((() => _currentTime > shootingTime - 0.75f));
            cannonBullet.rb.gravityScale = 0f;
            bulletObj.SetActive(true);
                    
            Debug.DrawLine(_startPoint,endPoint);
                    
            trajectory.UpdateDots(cannonBullet.Pos,_force);
            trajectory.Show();
            
            yield return new WaitUntil((() => _currentTime > shootingTime));
            Shooting();
            _currentTime = 0;
        }
        
    }

    void Shooting()
    {
        cannonBullet.Push(_force);
        trajectory.Hide();
    }
}
