using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNB
{
    /// <summary>
    /// Evaluates every "EvaluationFrequency" the distance from every GameObject with  "ActivateOnDistance.cs component" to Game Object tagged as "Player"
    /// Evalúa cada "Frecuencia de evaluación", la distancia desde cada GameObject con el "componente ActivateOnDistance.cs" hasta el Game Object etiquetado como "Player".
    /// 评估每个“EvaluationFrequency”从每个具有“ActivateOnDistance.cs组件”的游戏对象到标记为“玩家”的游戏对象的距离
    /// </summary>
    public class ActivateOnDistanceManager : MonoBehaviour
    {
        [HideInInspector]
        public bool AutomaticallySetPlayerAsTarget = false;
        [HideInInspector]
        public Transform ProximityTarget;
        [HideInInspector]
        public bool AutomaticallyGrabControlledObjects = true;
        [HideInInspector]
        public List<ActivateOnDistance> ControlledObjects;
        [HideInInspector]
        public float EvaluationFrequency = 0.5f;

        GameObject _spawnHolder;
        GameObject _spawnFreeCellHolder;

        protected float _lastEvaluationAt = 0f;

        protected virtual void Start()
        {
            GrabControlledObjects();
            _spawnHolder = GameObject.FindGameObjectWithTag("SpawnHolder");
            _spawnFreeCellHolder = GameObject.FindGameObjectWithTag("SpawnerFreeCellHolder");
        }

        protected virtual void GrabControlledObjects()
        {
            if (AutomaticallyGrabControlledObjects && _spawnHolder != null && _spawnFreeCellHolder != null)
            {
                List<GameObject> taggedGameObjects = new List<GameObject>();

                for (int i = 0; i < _spawnHolder.transform.childCount; i++)
                {
                    Transform child = _spawnHolder.transform.GetChild(i);
                    if (child.tag == "Distance Managed")
                    {
                        taggedGameObjects.Add(child.gameObject);
                    }
                }

                for (int i = 0; i < _spawnFreeCellHolder.transform.childCount; i++)
                {
                    Transform child = _spawnFreeCellHolder.transform.GetChild(i);
                    if (child.tag == "Distance Managed")
                    {
                        taggedGameObjects.Add(child.gameObject);
                    }
                }

                foreach (var item in taggedGameObjects)
                {
                    ControlledObjects.Add(item.GetComponent<ActivateOnDistance>());
                }
            }
        }

        public virtual void AddControlledObject(ActivateOnDistance newObject)
        {
            ControlledObjects.Add(newObject);
        }

        protected virtual void SetPlayerAsTarget()
        {
            if (AutomaticallySetPlayerAsTarget)
            {
                ProximityTarget = GameObject.FindGameObjectWithTag("Player")?.transform;
            }
        }

        protected virtual void Update()
        {
            if (!ProximityTarget)
            {
                SetPlayerAsTarget();
            }

            if (ControlledObjects.Count == 0)
            {
                GrabControlledObjects();
            }

            if (ProximityTarget)
            {
                EvaluateDistance();

            }
        }

        protected virtual void EvaluateDistance()
        {
            if (Time.time - _lastEvaluationAt > EvaluationFrequency)
            {
                _lastEvaluationAt = Time.time;

            }
            else
            {
                return;
            }
            foreach (ActivateOnDistance proxy in ControlledObjects)
            {
                if (proxy)
                {
                    float distance = Vector3.Distance(proxy.transform.position, ProximityTarget.position);
                    if (proxy.gameObject.activeInHierarchy && distance > proxy.DisableDistance)
                    {
                        proxy.gameObject.SetActive(false);
                        proxy.DisabledByManager = true;
                    }
                    if (!proxy.gameObject.activeInHierarchy && proxy.DisabledByManager && distance < proxy.EnableDistance)
                    {
                        proxy.gameObject.SetActive(true);
                        proxy.DisabledByManager = false;
                    }
                }
            }
        }
    }
}