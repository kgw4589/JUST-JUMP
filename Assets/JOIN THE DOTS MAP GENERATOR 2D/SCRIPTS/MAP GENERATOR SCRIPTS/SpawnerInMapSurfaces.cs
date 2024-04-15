using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace CNB
{
    /// <summary>
    /// Class in charge of creating new "game objects" along map surfaces, diferenciating floor, top, and rest of surfaces.
    /// Clase encargada de crear nuevos "game objects" a lo largo de las superficies del mapa diferenciando suelo, techo y resto de superficies.
    /// 负责沿着地图表面创建新的“游戏对象”的类，区分地板、天花板和其他表面
    /// </summary>
    [Serializable]
    public class SpawnerInMapSurfaces : MonoBehaviour
    {
        int _spawnDensity;
        bool _addColliders;
        float _scale;

        List<GameObject> objecstoToSpawn = new List<GameObject>();
        List<int> config = new List<int>();//1=floor, 2 = top, 3 = rest

        [HideInInspector]
        public bool _colliderSimple;

        //Variables set in the "MapSO.cs scriptable object asset"
        //Variables establecidas en el "MapSO scrittable object asset"
        //“MapSO scrittable object asset”中设置的变量
        [HideInInspector]
        public int _spawnDensityFloor;
        [HideInInspector]
        public bool addColliderFloor;
        [HideInInspector]
        public List<Sprite> spriteListFloor = new List<Sprite>();
        [HideInInspector]
        public float offsetFloor;
        [HideInInspector]
        public float spriteScaleFloor;

        [HideInInspector]
        public int _spawnDensityTop;
        [HideInInspector]
        public bool addColliderTop;
        [HideInInspector]
        public List<Sprite> spriteListTop = new List<Sprite>();
        [HideInInspector]
        public float offsetTop;
        [HideInInspector]
        public float spriteScaleTop;

        [HideInInspector]
        public int _spawnDensityRest;
        [HideInInspector]
        public bool addColliderRest;
        [HideInInspector]
        public List<Sprite> spriteListRest = new List<Sprite>();
        [HideInInspector]
        public float offsetRest;
        [HideInInspector]
        public float spriteScaleRest;
        //.....

        GameObject holder;
        MeshGeneratorCNB mesh;

        public void Generate(GameObject _map, VertexPath[] _creators, bool _spawnColliders)
        {
            if (holder == null)
            {
                holder = GameObject.FindGameObjectWithTag("SpawnHolder");
            }

            mesh = _map.GetComponent<MeshGeneratorCNB>();
            if (_spawnColliders)
            {
                SpriteRenderer rend;
                //objecstoToSpawn.Clear();
                List<GameObject> prefabListFloor = new List<GameObject>();
                for (int i = 0; i < spriteListFloor.Count; i++)
                {
                    prefabListFloor.Add(new GameObject("floor Spawner" + mesh._colliderSpawnersCount + "-" + i));
                    rend = prefabListFloor[i].AddComponent<SpriteRenderer>();
                    rend.sprite = spriteListFloor[i];
                    prefabListFloor[i].transform.localScale = Vector3.one * spriteScaleFloor;
                    prefabListFloor[i].transform.parent = holder.transform;
                    config.Add(1);
                    objecstoToSpawn.Add(prefabListFloor[i]);
                }
                List<GameObject> prefabListTop = new List<GameObject>();
                for (int i = 0; i < spriteListTop.Count; i++)
                {
                    prefabListTop.Add(new GameObject("top Spawner" + mesh._colliderSpawnersCount + "-" + i));
                    rend = prefabListTop[i].AddComponent<SpriteRenderer>();
                    rend.sprite = spriteListTop[i];
                    prefabListTop[i].transform.localScale = Vector3.one * spriteScaleTop;
                    prefabListTop[i].transform.parent = holder.transform;
                    config.Add(2);
                    objecstoToSpawn.Add(prefabListTop[i]);
                }
                List<GameObject> prefabListRest = new List<GameObject>();
                for (int i = 0; i < spriteListRest.Count; i++)
                {
                    prefabListRest.Add(new GameObject("rest Spawner" + mesh._colliderSpawnersCount + "-" + i));
                    rend = prefabListRest[i].AddComponent<SpriteRenderer>();
                    rend.sprite = spriteListRest[i];
                    prefabListRest[i].transform.localScale = Vector3.one * spriteScaleRest;
                    prefabListRest[i].transform.parent = holder.transform;
                    config.Add(3);
                    objecstoToSpawn.Add(prefabListRest[i]);
                }

                SpawnColliders(_creators);
            }
        }

        void ApplySpawnDensityAndColliderPrefsAndScale(int config)
        {
            if (config == 1)
            {
                _spawnDensity = _spawnDensityFloor;
                _addColliders = addColliderFloor;
                _scale = spriteScaleFloor;
            }
            else if (config == 2)
            {
                _spawnDensity = _spawnDensityTop;
                _addColliders = addColliderTop;
                _scale = spriteScaleTop;
            }
            else
            {
                _spawnDensity = _spawnDensityRest;
                _addColliders = addColliderRest;
                _scale = spriteScaleRest;
            }
            if (_spawnDensity==0)
            {
                Debug.LogWarning("spawn density must be set in MapSO, defaults to 5");
                _spawnDensity = 5;
            }
            if (_scale == 0)
            {
                Debug.LogWarning("scale must be set in MapSO, defaults to .5f");
                _scale = .5f;
            }
        }

        void SpawnColliders(VertexPath[] _creators)
        {
            if (_creators != null && objecstoToSpawn != null && holder != null)
            {

                for (int i = 0; i < _creators.Length; i++)
                {
                    VertexPath path = _creators[i];
                    for (int j = 0; j < path.NumPoints; j++)
                    {
                        Vector3 localNormal = transform.TransformPoint(-path.GetNormal(j));
                        Vector2 normal = new Vector2(localNormal.x, localNormal.y).normalized;
                        float angulo = Vector2.SignedAngle(normal, Vector2.up);
                        float dotProduct = Vector2.Dot(normal, Vector3.up);
                        Quaternion quaternion = Quaternion.AngleAxis(angulo < 180 ? -angulo : angulo, Vector3.forward);

                        int random = Random.Range(0, objecstoToSpawn.Count);
                        if (objecstoToSpawn.Count > 0)
                        {
                            GameObject obj = objecstoToSpawn[random];
                            int conf = config[random];
                            ApplySpawnDensityAndColliderPrefsAndScale(conf);
                            if (_spawnDensity == 0)
                            {
                                _spawnDensity = 1;
                            }
                            if (_scale == 0)
                            {
                                _scale = 1;
                            }
                            // spawn with rotation respect to "surface normal" in case 3 and respect to global normal in cases 1 and 2
                            // spawn de acuerdo con la rotación "surface normal" en el caso 3, y de acuerdo con "vector2.up" en los casos 1 y 2
                            // 在情况 3 中根据“表面法线”旋转生成，在情况 1 和 2 中根据“vector2.up”生成
                            if (j % _spawnDensity == 0)
                            {
                                if (dotProduct < 0)
                                {
                                    if (dotProduct <= -.9f && obj != null && conf == 2)//top
                                    {
                                        GameObject newGO = Instantiate(obj, path.GetPoint(j) - path.GetNormal(j) * offsetTop, Quaternion.Euler(0, 0, 180), holder.transform);
                                        AddColliderAndSetScaleAndZ(newGO, _addColliders);
                                        SetProximity(newGO);
                                    }
                                    else//rest
                                    {
                                        if (dotProduct > -.9f && obj != null && conf == 3)//rest
                                        {
                                            GameObject newGO = Instantiate(obj, path.GetPoint(j) - path.GetNormal(j) * offsetRest, quaternion, holder.transform);
                                            AddColliderAndSetScaleAndZ(newGO, _addColliders);
                                            SetProximity(newGO);
                                        }
                                    }
                                }
                                else
                                {
                                    if (dotProduct >= .9f && obj != null && conf == 1)//floor
                                    {

                                        GameObject newGO = Instantiate(obj, path.GetPoint(j) - path.GetNormal(j) * offsetFloor, Quaternion.identity, holder.transform);
                                        AddColliderAndSetScaleAndZ(newGO, _addColliders);
                                        SetProximity(newGO);

                                    }
                                    else//rest
                                    {
                                        if (dotProduct < .9f && obj != null && conf == 3)//rest
                                        {
                                            GameObject newGO = Instantiate(obj, path.GetPoint(j) - path.GetNormal(j) * offsetRest, quaternion, holder.transform);
                                            AddColliderAndSetScaleAndZ(newGO, _addColliders);
                                            SetProximity(newGO);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var obj in objecstoToSpawn)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }

        void AddColliderAndSetScaleAndZ(GameObject obj, bool add)
        {
            obj.transform.localScale = Vector3.one * _scale;
            obj.transform.localPosition = SetZ(obj.transform.localPosition, .5f);

            if (add)
            {
                if (_colliderSimple)
                {
                    obj.AddComponent<BoxCollider2D>();
                }
                else
                {
                    obj.AddComponent<PolygonCollider2D>();
                }
                int collLayer = LayerMask.NameToLayer("Collisions");
                obj.gameObject.layer = collLayer;
            }
        }

        void SetProximity(GameObject go)
        {
            ActivateOnDistance pm = go.AddComponent<ActivateOnDistance>();
            go.tag = "DistanceManaged";
            go.SetActive(true);
        }

        Vector3 SetZ(Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }
    }
}