using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMil : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 0.5f;
    private void Update()
    {
        if (Time.timeScale == 0) return;
        transform.Rotate(new Vector3(0,0,turnSpeed));
    }
}
