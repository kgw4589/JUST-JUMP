using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    public float WaveDistance { get; private set; }
    
    [SerializeField] private float _waitTime = 5f; // wait second
    [SerializeField] private float _moveDistance = 10000f; // Max Y Axis
    [SerializeField] private float _moveDuration = 10000f;  // second
    
    private float _soundDistance = 25f;

    private float _startYaxis;
    private float _contactTime = 0f;
    private float _timeSecond = 1f; // 1 Second +
    private int _contactCnt = 0;

    private Player _player;

    private void Awake()
    {
        GameManager.Instance.initAction += InitObject;
        GameManager.Instance.startAction += StartMove;

        _startYaxis = transform.position.y;
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
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            WaveSound();
        }
    }

    private void WaveSound()
    {
        WaveDistance = _player.transform.position.y - transform.position.y;
        
        if (WaveDistance < _soundDistance)
        {
            float waveVolume = Mathf.Clamp01(1 - (WaveDistance / _soundDistance));
            SoundManager.Instance.SetWaveVolume(waveVolume);
            SoundManager.Instance.PlayWaveSound(true);
        }
        else
        {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlayWaveHitSound(0.25f);
        }
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
            
            SoundManager.Instance.PlayWaveHitSound(1f);
        }
    }
}
