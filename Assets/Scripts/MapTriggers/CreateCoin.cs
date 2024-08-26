using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CreateCoin : MonoBehaviour
{
    public GameObject[] coin;
    [Header("최대로 생성될 코인 개수")]
    public int maxCoin;             // 3이면 1~3개 사이로 나옴
    private readonly List<int> _coinList = new List<int>();

    private void Awake()
    {
        foreach (var t in coin)
        {
            t.SetActive(false);
        }
    }
    private void Start()
    {
        for(int l = 0; l < maxCoin; l++)
        {
            CreateUnDuplicateRandom(0, coin.Length - 1);
        }
    
        foreach (var i in _coinList)
        {
            Debug.Log(i);
            coin[i].SetActive(true);
        }
    }
    
    // 랜덤 생성 (중복 배제)
    private void CreateUnDuplicateRandom(int min, int max)
    {
        int currentNumber = Random.Range(min, max);
        _coinList.Add(currentNumber);
    }
}
