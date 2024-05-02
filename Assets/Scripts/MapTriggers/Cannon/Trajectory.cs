using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private int dotNumber;
    [SerializeField] private GameObject dotParents;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private float dotSpacing;

    private Transform[] _dotsList;
    
    private Vector2 _pos;
    
    private float _timeStamp;

    private void Start()
    {
        Hide();
        
        PrepareDots();
    }

    private void PrepareDots()
    {
        _dotsList = new Transform[dotNumber];

        for (int i = 0; i < dotNumber; i++)
        {
            _dotsList[i] = Instantiate(dotPrefab, null).transform;
            _dotsList[i].parent = dotParents.transform;
        }
    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    {
        _timeStamp = dotSpacing;
        for (int i = 0; i < dotNumber; i++)
        {
            _pos.x = (ballPos.x + forceApplied.x * _timeStamp);
            _pos.y = (ballPos.y + forceApplied.y * _timeStamp)
                     - (Physics2D.gravity.magnitude * _timeStamp * _timeStamp) / 2;

            _dotsList[i].position = _pos;
            _timeStamp += dotSpacing;
        }
    }

    public void Show()
    {
        dotParents.SetActive(true);
    }
    public void Hide()
    {
        dotParents.SetActive(false);
    }
}