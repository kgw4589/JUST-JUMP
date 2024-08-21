using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    [SerializeField] private float _waitTime = 5f; // wait second
    [SerializeField] private float _moveDistance = 100f; // Max Y Axis
    [SerializeField] private float _moveDuration = 100f;  // second

    private float _startYaxis = -30f;
    private float _contactTime = 0f;
    private float _timeSecond = 2f; // 1 Second +
    private int _contactCnt = 0;

    private Player _player;

    private void Awake()
    {
        GameManager.Instance.initAction += InitObject;
        GameManager.Instance.startAction += StartMove;
    }

    void InitObject()
    {
        transform.position = new Vector3(0, _startYaxis, -1);
        StartCoroutine(MoveUpAfterWait());
    }

    void StartMove()
    {
        StartCoroutine(MoveUpAfterWait());
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    IEnumerator MoveUpAfterWait()
    {
        float _timeConsol = _waitTime;

        while (_timeConsol > 0)
        {
            Debug.Log("Wave Wait: " + _timeConsol + "(s)");
            yield return new WaitForSeconds(1f);
            _timeConsol -= 1f;
        }
        
        // yield return new WaitForSeconds(_waitTime); Not Consol log

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + _moveDistance, startPosition.z);

        float _elapsedTime = 0f;
        while (_elapsedTime < _moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (_elapsedTime / _moveDuration));
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // _contactTime += Time.deltaTime;
            //
            // if (_contactTime >= _timeSecond)
            // {
            //     _player.curruentTime = 0;
            //     _contactCnt++;
            //     _player.Damage();
            //     Debug.Log($"Player Cnt : {_contactCnt}(s)");
            //     _contactTime = 0f;
            // }
            _player.Damage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.isHitWave = false;
            
            _contactTime = 0f;
            _contactCnt = 0;
        }
    }
}
