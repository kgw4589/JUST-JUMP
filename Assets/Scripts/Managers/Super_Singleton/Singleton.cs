using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
    
                if (_instance == null)
                {
                    GameObject gameObject = new GameObject(typeof(T).Name, typeof(T));
                    _instance = gameObject.GetComponent<T>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (transform.parent != null && transform.root != null)
        {
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        else
        {
             DontDestroyOnLoad(this.gameObject);
        }
    }
}
