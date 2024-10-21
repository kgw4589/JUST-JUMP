using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    [SerializeField] private float _waitTime = 5f; // wait second
    [SerializeField] private float _moveDistance = 10000f; // Max Y Axis
    [SerializeField] private float _moveDuration = 10000f;  // second
    
    private float _soundDistance = 10f;
    private float s;
    
    private float _startYaxis = -30f;
    private float _contactTime = 0f;
    private float _timeSecond = 1f; // 1 Second +
    private int _contactCnt = 0;

    private Player _player;

    private Vector3 temp;

    private void Awake()
    {
        GameManager.Instance.initAction += InitObject;
        GameManager.Instance.startAction += StartMove;
    }

    void InitObject()
    {
        transform.position = new Vector3(0, _startYaxis, -1);
    }

    void StartMove()
    {
        StartCoroutine(MoveUpAfterWait());
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        float s = transform.localScale.y / 2;
        
    }

    void Update()
    {
        temp = transform.position + new Vector3(0, s+26, 0);
        float distance = _player.transform.position.y - temp.y;
        if (distance < _soundDistance)
        {
            //Debug.Log(distance);
            Debug.Log("파도 효과음 재생");
            SoundManager.Instance.PlayWaveSound(true);
            if (distance <= 0)
            {
                Debug.Log("피치 낮아짐");
                SoundManager.Instance.PlayWaveHitSound(0.25f);
            }
            else
            {
                SoundManager.Instance.PlayWaveHitSound(1f);
            }
        }
        else
        {
            Debug.Log("브금 끔");
            SoundManager.Instance.PlayWaveSound(false);
        }
    }

    IEnumerator MoveUpAfterWait()
    {
        yield return new WaitForSeconds(MapManager.Instance.selectedMapScriptable.waitTime);

        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + _moveDistance, startPosition.z);

        float _elapsedTime = 0f; // Elapsed Time
        float _currentSpeed = 0f; // speed
        float _maxSpeed = MapManager.Instance.selectedMapScriptable.maxSpeed;
        float _accel = MapManager.Instance.selectedMapScriptable.moveDistance
                       / MapManager.Instance.selectedMapScriptable.moveDuration; // accelerlation
        
        while (_elapsedTime < _moveDuration)
        {
            // Max Speed Lock
            if (_currentSpeed > _maxSpeed)
            {
                _currentSpeed = _maxSpeed;
            }
            
            _currentSpeed += _accel * Time.deltaTime; // speed Up
            transform.position = Vector3.MoveTowards(transform.position, endPosition, _currentSpeed * Time.deltaTime);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
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
