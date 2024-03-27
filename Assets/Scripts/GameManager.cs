using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private int _score;
    [SerializeField] private float _playerYPos;
    
    [SerializeField] private int _highScore;
    
    void Awake()
    {
        _highScore = PlayerPrefs.GetInt("highScore", 0);
    }
    
    void Update()
    {
        
    }
    
    
}
