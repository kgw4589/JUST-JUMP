using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    private MapScriptable _mapScriptable;
    
    private Vector2 _startPos;
    
    private GameObject _player;
    private GameObject _gameOverZone;
    
    private GameObject _lastMap;
    private Vector3 _interval;
    private float _mapSizeY;
    
    private Queue<GameObject> _mapPosQueue = new Queue<GameObject>();
    
    private bool _isInitComplete = false;
    
    private int _mapSectionIndex = 0;
    
    private float _mapDestroyDistance = 50.0f;
    
    public enum MapMode
    {
        Normal
    }
    public MapMode mapMode = MapMode.Normal;

    public void InitMap()
    {
        _player = FindObjectOfType<Player>().gameObject;
        _gameOverZone = FindObjectOfType<GameOverZone>().gameObject;
        
        _mapScriptable = Resources.Load<MapScriptable>("MapScriptables/" + mapMode);

        _mapSectionIndex = 0;
        
        _startPos = new Vector2(0, 0);
        
        _lastMap = Instantiate(_mapScriptable.maps[_mapSectionIndex]
            .sectionMaps[Random.Range(0, _mapScriptable.maps[_mapSectionIndex].sectionMaps.Count)]);
        
        _lastMap.transform.position = _startPos;
        _mapPosQueue.Enqueue(_lastMap);

        _isInitComplete = true;
        
        InstantiateRandomMap();
        InstantiateRandomMap();
    }

    public void EndMap()
    {
        _mapPosQueue.Clear();
        _isInitComplete = false;
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
        
        Vector3 mapQueFirst = _mapPosQueue.Peek().transform.position;

        if (_gameOverZone.transform.position.y - mapQueFirst.y > _mapDestroyDistance)
        {
            Destroy(_mapPosQueue.Dequeue());
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
        
        _mapPosQueue.Enqueue(_lastMap);
    }
}