using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] maps;
    
    [SerializeField] private Vector2 startPos;
    
    private GameObject _lastMap;
    private Vector3 _interval;
    
    void Start()
    {
        _interval = new Vector2(0, maps[0].transform.localScale.y);
        
        _lastMap = Instantiate(maps[Random.Range(0, maps.Length)]);
        _lastMap.transform.position = startPos;
        InstantiateRandomMap();
        InstantiateRandomMap();
    }

    void InstantiateRandomMap()
    {
        GameObject map = Instantiate(maps[Random.Range(0, maps.Length)]);
        map.transform.position = _lastMap.transform.position += _interval;
    }
}