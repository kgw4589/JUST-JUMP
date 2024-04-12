using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNB
{
    /// <summary>
    /// Class in charge of creating new "game objects" in map coordinates that are free.
    /// Clase encargada de crear nuevos "game objects" en coordenadas del mapa que estén libres.
    /// 负责在自由地图坐标中创建新“游戏对象”的类。
    /// </summary>
    [Serializable]
    public class SpawnerInFreeMapCells : MonoBehaviour
    {
        //Variables set in the "MapSO.cs scriptable object asset"
        //Variables establecidas en el "MapSO scrittable object asset"
        //“MapSO scrittable object asset”中设置的变量
        [HideInInspector]
        public float radius; 
        [HideInInspector]
        public bool addCollider;
        [HideInInspector]
        public bool colliderSimple;
        [HideInInspector]
        public float spritesScale;
        [HideInInspector]
        public List<Sprite> _spritesGO;
        //.......


        [HideInInspector]
        public Vector2 regionSize;
        int rejectionSamples = 10;
        MapGeneratorCNB _mapGen;
        [HideInInspector]
        public List<Vector2> points;
        GameObject _holder;

        public void Init()
        {
            _mapGen = FindObjectOfType<MapGeneratorCNB>();
            _holder = GameObject.FindGameObjectWithTag("SpawnerFreeCellHolder");

            if (_mapGen)
            {
                ActualizeSize();
                points = GeneratePoints(radius, regionSize, rejectionSamples);
            }
        }

        public void Spawn(List<Vector2> puntos)
        {
            SpriteRenderer rend;
            GameObject newFreeCellObj;
            _mapGen._freeCellSpawnedGOGlobalCount = 0;
            if (puntos != null && _spritesGO.Count > 0)
            {
                foreach (Vector2 point in puntos)
                {
                    int index = UnityEngine.Random.Range(0, _spritesGO.Count);
                    newFreeCellObj = new GameObject("Freecell Spawner" + _mapGen._freeCellSpawnersCount +"-"+ _mapGen._freeCellSpawnedGOGlobalCount);
                    _mapGen._freeCellSpawnedGOGlobalCount++;
                    rend = newFreeCellObj.AddComponent<SpriteRenderer>();
                    rend.sprite = _spritesGO[index];

                    if (rend.sprite != null)
                    {
                        ActivateOnDistance proxMan = newFreeCellObj.AddComponent<ActivateOnDistance>();
                        if (addCollider)
                        {
                            if (colliderSimple)
                            {
                                BoxCollider2D coll = newFreeCellObj.AddComponent<BoxCollider2D>();
                            }
                            else
                            {
                                PolygonCollider2D coll = newFreeCellObj.AddComponent<PolygonCollider2D>();
                            }
                        }
                        int collLayer = LayerMask.NameToLayer("Collisions");
                        newFreeCellObj.gameObject.layer = collLayer;
                        newFreeCellObj.tag = "DistanceManaged";
                        newFreeCellObj.transform.position = point;
                        newFreeCellObj.transform.localScale = Vector3.one * (spritesScale==0?.5f: spritesScale);
                        newFreeCellObj.transform.parent = _holder.transform;
                        newFreeCellObj.transform.localPosition = SetZ(newFreeCellObj.transform.localPosition, 1);
                        newFreeCellObj.SetActive(true);
                    }
                }
            }
        }

        void ActualizeSize()
        {
            regionSize = new Vector2(_mapGen._map._width, _mapGen._map._height) * _mapGen._map._squareSize;
        }

        Vector3 SetZ(Vector2 vector, float z)
        {
            Vector3 v = new Vector3(vector.x, vector.y, z);
            return v;
        }

        List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30)
        {
            if (radius==0)
            {
                radius = 5;
            }

            float cellSize = radius / Mathf.Sqrt(2);

            int[,] grid = new int[Mathf.CeilToInt(sampleRegionSize.x / cellSize), Mathf.CeilToInt(sampleRegionSize.y / cellSize)];
            List<Vector2> points = new List<Vector2>();
            List<Vector2> spawnPoints = new List<Vector2>();

            spawnPoints.Add(sampleRegionSize / 2);
            while (spawnPoints.Count > 0)
            {
                int spawnIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
                Vector2 spawnCentre = spawnPoints[spawnIndex];
                bool candidateAccepted = false;

                for (int i = 0; i < numSamplesBeforeRejection; i++)
                {
                    float angle = UnityEngine.Random.value * Mathf.PI * 2;
                    Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                    Vector2 candidate = spawnCentre + dir * UnityEngine.Random.Range(radius, 2 * radius);
                    if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid))
                    {
                        points.Add(candidate);
                        spawnPoints.Add(candidate);
                        grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                        candidateAccepted = true;
                        break;
                    }
                }
                if (!candidateAccepted)
                {
                    spawnPoints.RemoveAt(spawnIndex);
                }
            }
            return points;
        }

        bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid)
        {
            if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && candidate.y >= 0 && candidate.y < sampleRegionSize.y)
            {
                int cellX = (int)(candidate.x / cellSize);
                int cellY = (int)(candidate.y / cellSize);
                int searchStartX = Mathf.Max(0, cellX - 2);
                int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
                int searchStartY = Mathf.Max(0, cellY - 2);
                int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);

                for (int x = searchStartX; x <= searchEndX; x++)
                {
                    for (int y = searchStartY; y <= searchEndY; y++)
                    {
                        int pointIndex = grid[x, y] - 1;
                        if (pointIndex != -1)
                        {
                            float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                            if (sqrDst < radius * radius)
                            {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}