using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    private MapListScriptable _mapListScriptable;
    private MapScriptable _selectedMapScriptable;
    
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

    private void Awake()
    {
        GameManager.Instance.mapManager = this;
        
        _mapSectionIndex = 0;
        _mapScriptableIndex = 0;

        _mapListScriptable = Resources.Load<MapListScriptable>("MapScriptables");
        _selectedMapScriptable = _mapListScriptable.mapScriptableList[_mapScriptableIndex];
        modeText.text = "MODE : " + _selectedMapScriptable.name;
        
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

        _mapSectionIndex = 0;
        _mapQueue.Clear();
    }

    private void StartMap()
    {
        _player ??= FindObjectOfType<Player>().gameObject;
        _gameOverZone ??= FindObjectOfType<GameOverZone>().gameObject;
        
        _lastMap = Instantiate(_selectedMapScriptable.maps[_mapSectionIndex]
            .sectionMaps[Random.Range(0, _selectedMapScriptable.maps[_mapSectionIndex].sectionMaps.Count)]);

        _lastMap.transform.position = _startPos + new Vector2(0, _lastMap.transform.localScale.y / 2);
        _mapQueue.Enqueue(_lastMap);

        _isInitComplete = true;
        
        InstantiateRandomMap();
        InstantiateRandomMap();
    }

    public void ChangeMode(int index)
    {
        _selectedMapScriptable = _mapListScriptable.mapScriptableList[index];
        
        modeText.text = "MODE : " + _selectedMapScriptable.name;
    }

    public void ChangeMode(bool isMinus)
    {
        _mapScriptableIndex = Mathf.Abs(_mapScriptableIndex + (isMinus ? -1 : 1)) % _mapListScriptable.mapScriptableList.Count;
        
        _selectedMapScriptable = _mapListScriptable.mapScriptableList[_mapScriptableIndex];
        
        modeText.text = "MODE : " + _selectedMapScriptable.name;
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
            if (_mapSectionIndex < _selectedMapScriptable.section.Length
                && GameManager.Instance.playerPosY >= _selectedMapScriptable.section[_mapSectionIndex])
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
        GameObject map = Instantiate(_selectedMapScriptable.maps[_mapSectionIndex]
            .sectionMaps[Random.Range(0, _selectedMapScriptable.maps[_mapSectionIndex].sectionMaps.Count)]);
        
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