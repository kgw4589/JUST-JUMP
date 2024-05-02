using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapScriptable mapScriptable;
    
    [SerializeField] private Vector2 startPos;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameOverZone;

    private GameObject _lastMap;
    private Vector3 _interval;
    private float _mapSizeY;

    private Queue<GameObject> _mapPosQueue = new Queue<GameObject>();
    
    public enum MapMode
    {
        Normal
    }
    public MapMode mapMode;

    void Start()
    {
        mapScriptable = Resources.Load<MapScriptable>("MapScriptables/" + mapMode);
        
        _lastMap = Instantiate(mapScriptable.maps[Random.Range(0, mapScriptable.maps.Count)]);
        _lastMap.transform.position = startPos;
        _mapPosQueue.Enqueue(_lastMap);
        
        InstantiateRandomMap();
        InstantiateRandomMap();
    }

    void Update()
    {
        float dis = Vector2.Distance(player.transform.position, _lastMap.transform.position);

        if (dis < _mapSizeY * 3)
        {
            InstantiateRandomMap();
        }
        
        Vector3 mapQueFirst = _mapPosQueue.Peek().transform.position;

        if (gameOverZone.transform.position.y - mapQueFirst.y > _mapSizeY)
        {
            Destroy(_mapPosQueue.Dequeue());
        }
    }

    void InstantiateRandomMap()
    {
        GameObject map = Instantiate(mapScriptable.maps[Random.Range(0, mapScriptable.maps.Count)]);
        
        float lastMapSizeY = _lastMap.transform.localScale.y / 2;
        _mapSizeY = map.transform.localScale.y / 2;
        
        _interval = new Vector2(0, lastMapSizeY + _mapSizeY);

        map.transform.position = _lastMap.transform.position + _interval;
        _lastMap = map;
        
        _mapPosQueue.Enqueue(_lastMap);
    }
}