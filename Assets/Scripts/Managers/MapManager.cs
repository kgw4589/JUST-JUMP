using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapManager : Singleton<MapManager>
{
    private MapListScriptable _mapScriptables;
    private MapScriptable _mapScriptable;
    
    private Vector2 _startPos;
    
    private GameObject _player;
    private GameObject _gameOverZone;
    
    private GameObject _lastMap;
    private Vector3 _interval;
    private float _mapSizeY;
    
    private Queue<GameObject> _mapQueue = new Queue<GameObject>();
    
    private bool _isInitComplete = false;

    private int _mapScriptableIndex = 0;
    private int _mapSectionIndex = 0;

    private float _mapDestroyDistance = 50.0f;

    [SerializeField] private Text modeText;

    protected override void Init()
    {
        _mapSectionIndex = 0;
        _mapScriptableIndex = 0;

        _mapScriptables = Resources.Load<MapListScriptable>("MapScriptables");
        _mapScriptable = _mapScriptables.mapScriptableList[_mapScriptableIndex];
        
        _startPos = new Vector2(0, 0);
        
        GameManager.Instance.initAction += InitObject;
        GameManager.Instance.startAction += StartMap;
        
        InitObject();
    }

    public void InitObject()
    {
        _isInitComplete = false;

        foreach (var map in _mapQueue)
        {
            Destroy(map);
        }
        
        _mapQueue.Clear();
    }

    public void StartMap()
    {
        _player ??= FindObjectOfType<Player>().gameObject;
        _gameOverZone ??= FindObjectOfType<GameOverZone>().gameObject;
        
        _lastMap = Instantiate(_mapScriptable.maps[_mapSectionIndex]
            .sectionMaps[Random.Range(0, _mapScriptable.maps[_mapSectionIndex].sectionMaps.Count)]);

        _lastMap.transform.position = _startPos + new Vector2(0, _lastMap.transform.localScale.y / 2);
        _mapQueue.Enqueue(_lastMap);

        _isInitComplete = true;
        
        InstantiateRandomMap();
        InstantiateRandomMap();
    }

    public void ChangeMode(bool isMinus)
    {
        if (isMinus)
        {
            _mapScriptableIndex = (_mapScriptableIndex == 0) ?
                    _mapScriptables.mapScriptableList.Count - 1 : --_mapScriptableIndex;
        }
        else
        {
            _mapScriptableIndex = (_mapScriptableIndex + 1) % _mapScriptables.mapScriptableList.Count;
        }

        _mapScriptable = _mapScriptables.mapScriptableList[_mapScriptableIndex];
        modeText.text = "MODE : " + _mapScriptable.name;
        Debug.Log(_mapScriptable);
    }

    void Update()
    {
        if (!_isInitComplete)
        {
            return;
        }

        float dis = Vector2.Distance(_player.transform.position, _lastMap.transform.position);

        if (dis < _mapSizeY * 3)
        {
            if (_mapSectionIndex < _mapScriptable.section.Length
                && GameManager.Instance._playerPosY >= _mapScriptable.section[_mapSectionIndex])
            {
                ++_mapSectionIndex;
            }
            InstantiateRandomMap();
        }
        
        Vector3 mapQueFirst = _mapQueue.Peek().transform.position;

        if (_gameOverZone.transform.position.y - mapQueFirst.y > _mapDestroyDistance)
        {
            Destroy(_mapQueue.Dequeue());
        }
    }

    void InstantiateRandomMap()
    {
        GameObject map = Instantiate(_mapScriptable.maps[_mapSectionIndex]
            .sectionMaps[Random.Range(0, _mapScriptable.maps[_mapSectionIndex].sectionMaps.Count)]);
        
        float lastMapSizeY = _lastMap.transform.localScale.y / 2;
        _mapSizeY = map.transform.localScale.y / 2;
        
        _interval = new Vector2(0, lastMapSizeY + _mapSizeY);

        map.transform.position = _lastMap.transform.position + _interval;
        _lastMap = map;
        
        _mapQueue.Enqueue(_lastMap);
    }

    private void OnDestroy()
    {
        GameManager.Instance.initAction -= InitObject;
    }
}