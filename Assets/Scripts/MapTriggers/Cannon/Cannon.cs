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

    [SerializeField][Range(0.1f, 15f)] private float shootingTime = 5f;
    [SerializeField] private float pushForce = 3f;
    
    [SerializeField] private CannonBullet cannonBullet;
    [SerializeField] private GameObject bulletObj;
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private Vector2 endPoint;
    [SerializeField] private GameObject cannonHead;
    private Vector2 _startPoint;
    private Vector2 _direction;
    private Vector2 _force;
    private float _startRot;

    private void Start()
    {
        _startPoint = transform.position;
        _startRot = transform.position.z;
        endPoint = new Vector2(endPoint.x + _startPoint.x, endPoint.y + _startPoint.y);
        
        Vector3 dir = endPoint - new Vector2(cannonHead.transform.position.x,cannonHead.transform.position.y);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannonHead.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward) * Quaternion.Euler(0, 0, 180);
        StartCoroutine(Reload());
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        _currentTime += Time.deltaTime;
        
        _bulletDistance = Vector2.Distance(transform.position, bulletObj.transform.position);
        
        if (_bulletDistance > 10f)
        {
            StartCoroutine(Reload());
            bulletObj.SetActive(false);
            bulletObj.transform.position = transform.position;
            _currentTime = 0;
        }
    }

    private IEnumerator Reload()
    {
        trajectory.Hide();
        _distance = Vector2.Distance(_startPoint, endPoint);
        _direction = (_startPoint - endPoint).normalized;
        _force = _direction * (_distance * pushForce);

        Debug.DrawLine(_startPoint, endPoint);

        yield return new WaitUntil((() => _currentTime > shootingTime - 0.75f));
        cannonBullet.rb.gravityScale = 0f;
        bulletObj.SetActive(true);

        Debug.DrawLine(_startPoint, endPoint);

        trajectory.UpdateDots(cannonBullet.Pos, _force);
        trajectory.Show();

        yield return new WaitUntil((() => _currentTime > shootingTime));
        Shooting();
        _currentTime = 0;
    }

    void Shooting()
    {
        cannonBullet.Push(_force);
    }
}
