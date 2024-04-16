using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

namespace CNB
{
    /// <summary>
    /// Generates a new  "map" and a new "mesh" from the data in "_mapSOLoadFrom" only if  (_mapSOLoadFrom.inEditMode==FALSE)
    /// Genera un nuevo "mapa" y una nueva "malla" a partir de los datos en "_mapSOLoadFrom" solo si (_mapSOLoadFrom.inEditMode==FALSE)
    /// 仅当 (_mapSOLoadFrom.inEditMode==FALSE) 时，才从“_mapSOLoadFrom”中的数据生成新的“地图”和新的“网格”
    /// </summary>
    [ExecuteInEditMode]
    public class MapGeneratorCNB : MonoBehaviour
    {

        [Serializable]
        public class Map
        {
            public int _width;
            public int _height;
            public int _squareSize;

            public string _seed;

            public int _tunnelRadio;

            public int _smoothMapIterations; 

            public int _randomFillPercent;

            public List<SpawnerInFreeMapCells> _spawnerFreeCell;

            public Material _backgroundMat;
            public Material _mapMeshMat;
            public Material _mapLineRendMat;

        }
        [HideInInspector]
        public Map _map;

        [HideInInspector]
        public int _borderMap;

        [HideInInspector]
        public GameObject _mapMesh;
        [HideInInspector]
        public GameObject _mapBackground;
        [HideInInspector]
        public GameObject _SpawnedHolder;
        [HideInInspector]
        public GameObject _spawnedFreeCellHolder;
        [HideInInspector]
        public GameObject _mapBounds;
        //...........
        List<List<Coord>> _freepos;
        //..............
        [HideInInspector]
        public GameObject _posToSpawnPlayer;

        public int[,] _mapArray;

        //SO
        public MapSO _mapSOLoadFrom;

        [HideInInspector]
        public MeshGeneratorCNB _meshGen;

        public bool _spawnFreeCellBool = false;

        SpawnerInFreeMapCells[] _spawnersFC;
        SpawnerInMapSurfaces[] _spawnersMS;

        List<string> _tagsNeeded = new List<string>();
        bool _tagsInitialized = false;
        List<string> _layersNeeded = new List<string>();
        bool _layersInitialized = false;

        [HideInInspector]
        public int _freeCellSpawnersCount = 0;
        [HideInInspector]
        public int _freeCellSpawnedGOGlobalCount = 0;

        public void LoadMap()
        {
            if (!_mapSOLoadFrom.inEditMode)
            {
                if (_mapSOLoadFrom != null && _meshGen != null)
                {
                    MapSO.MapScriptO map = _mapSOLoadFrom._map;
                    _borderMap = _mapSOLoadFrom._borderMap;
                    _spawnersFC = GetComponents<SpawnerInFreeMapCells>();
                    _spawnersMS = GetComponents<SpawnerInMapSurfaces>();

                    for (int i = 0; i < _spawnersFC.Length; i++)
                    {
                        DestroyImmediate(_spawnersFC[i], true);
                    }

                    for (int i = 0; i < _spawnersMS.Length; i++)
                    {
                        DestroyImmediate(_spawnersMS[i], true);
                    }
                    _map = new Map();

                    MapSO.MapScriptO _mapSOScriptO = _mapSOLoadFrom._map;
                    //map
                    _map._width = _mapSOScriptO.width;
                    _map._height = _mapSOScriptO.height;
                    _map._squareSize = _mapSOScriptO.squareSize;
                    _map._seed = _mapSOScriptO.seed.ToString();
                    _map._tunnelRadio = _mapSOScriptO.tunnelRadio;
                    _map._smoothMapIterations = _mapSOScriptO.smoothMapIterations;
                    if (_mapSOLoadFrom._mapMatSO==null)
                    {
                        Debug.LogWarning("MapMatSO scriptable object must be created and asigned in MapSO");
                    }
                    _map._backgroundMat = _mapSOLoadFrom._mapMatSO._backgroundMaterial;
                    _map._mapMeshMat = _mapSOLoadFrom._mapMatSO._meshMaterial;
                    _map._mapLineRendMat = _mapSOLoadFrom._mapMatSO._lineRendererMaterial;
                    _map._randomFillPercent = _mapSOScriptO.randomFillPercent;
                    //mesh
                    _meshGen.tileAmount = _mapSOScriptO.meshTextureScale;
                    _meshGen.lineRendererTextureTilling = _mapSOScriptO.lineRendererTextureTilling;
                    _meshGen.lineRendererVertexSpacing = _mapSOScriptO.lineRendererVertexSpacing;
                    //spawners
                    _map._spawnerFreeCell = new List<SpawnerInFreeMapCells>();
                    for (int j = 0; j < _mapSOScriptO._freeCellSpawners.Count; j++)
                    {
                        SpawnerInFreeMapCells newSP = gameObject.AddComponent<SpawnerInFreeMapCells>();
                        newSP.radius = _mapSOScriptO._freeCellSpawners[j].radius;
                        newSP.addCollider = _mapSOScriptO._freeCellSpawners[j].addCollider;
                        newSP.colliderSimple = _mapSOScriptO._freeCellSpawners[j].colliderSimple;
                        newSP.spritesScale = _mapSOScriptO._freeCellSpawners[j].spritesScale;

                        newSP._spritesGO = new List<Sprite>();
                        for (int k = 0; k < _mapSOScriptO._freeCellSpawners[j]._sprites.Count; k++)
                        {
                            newSP._spritesGO.Add(_mapSOScriptO._freeCellSpawners[j]._sprites[k]);
                        }

                        _map._spawnerFreeCell.Add(newSP);
                    }

                    _meshGen._spawnerCollider = new SpawnerInMapSurfaces[_mapSOScriptO._colliderSpawners.Count];
                    for (int l = 0; l < _mapSOScriptO._colliderSpawners.Count; l++)
                    {
                        SpawnerInMapSurfaces newCS = gameObject.AddComponent<SpawnerInMapSurfaces>();
                        _meshGen._spawnerCollider[l] = newCS;
                        _meshGen._spawnerCollider[l]._colliderSimple = _mapSOScriptO._colliderSpawners[l].colliderSimple;
                        _meshGen._spawnerCollider[l]._spawnDensityFloor = _mapSOScriptO._colliderSpawners[l].spawnDensityFloor;
                        _meshGen._spawnerCollider[l].addColliderFloor = _mapSOScriptO._colliderSpawners[l].addCollidersFloor;
                        _meshGen._spawnerCollider[l]._spawnDensityTop = _mapSOScriptO._colliderSpawners[l].spawnDensityTop;
                        _meshGen._spawnerCollider[l].addColliderTop = _mapSOScriptO._colliderSpawners[l].addCollidersTop;
                        _meshGen._spawnerCollider[l]._spawnDensityRest = _mapSOScriptO._colliderSpawners[l].spawnDensityRest;
                        _meshGen._spawnerCollider[l].addColliderRest = _mapSOScriptO._colliderSpawners[l].addCollidersRest;
                        _meshGen._spawnerCollider[l].offsetFloor = _mapSOScriptO._colliderSpawners[l].offsetFloor;
                        _meshGen._spawnerCollider[l].offsetTop = _mapSOScriptO._colliderSpawners[l].offsetTop;
                        _meshGen._spawnerCollider[l].offsetRest = _mapSOScriptO._colliderSpawners[l].offsetRest;
                        _meshGen._spawnerCollider[l].spriteScaleFloor = _mapSOScriptO._colliderSpawners[l].scaleFloor;
                        _meshGen._spawnerCollider[l].spriteScaleTop = _mapSOScriptO._colliderSpawners[l].scaleTop;
                        _meshGen._spawnerCollider[l].spriteScaleRest = _mapSOScriptO._colliderSpawners[l].scaleRest;

                        _meshGen._spawnerCollider[l].spriteListFloor = new List<Sprite>();
                        for (int m = 0; m < _mapSOScriptO._colliderSpawners[l].spriteListFloor.Count; m++)
                        {
                            if (_mapSOScriptO._colliderSpawners[l].spriteListFloor[m] != null)
                            {
                                _meshGen._spawnerCollider[l].spriteListFloor.Add(Instantiate(_mapSOScriptO._colliderSpawners[l].spriteListFloor[m]));
                            }
                        }

                        _meshGen._spawnerCollider[l].spriteListTop = new List<Sprite>();
                        for (int m = 0; m < _mapSOScriptO._colliderSpawners[l].spriteListTop.Count; m++)
                        {
                            if (_mapSOScriptO._colliderSpawners[l].spriteListTop[m] != null)
                            {
                                _meshGen._spawnerCollider[l].spriteListTop.Add(Instantiate(_mapSOScriptO._colliderSpawners[l].spriteListTop[m]));
                            }
                        }

                        _meshGen._spawnerCollider[l].spriteListRest = new List<Sprite>();
                        for (int m = 0; m < _mapSOScriptO._colliderSpawners[l].spriteListRest.Count; m++)
                        {
                            if (_mapSOScriptO._colliderSpawners[l].spriteListRest[m] != null)
                            {
                                _meshGen._spawnerCollider[l].spriteListRest.Add(Instantiate(_mapSOScriptO._colliderSpawners[l].spriteListRest[m]));
                            }
                        }
                    }
                }
            }
            GenerateMap(_mapSOLoadFrom.inEditMode);
        }

        private void Awake()
        {
            _tagsNeeded.Add("DistanceManaged");
            _tagsNeeded.Add("SpawnHolder");
            _tagsNeeded.Add("SpawnerFreeCellHolder");
            _tagsNeeded.Add("SpawnPlayerPos");
            _tagsNeeded.Add("Line Renderer");
            _tagsNeeded.Add("Map");
            _tagsNeeded.Add("MapSO");
            _tagsNeeded.Add("MapBounds");
            _tagsNeeded.Add("BackGround");

            _layersNeeded.Add("Collisions");

            if (!_tagsInitialized)
            {
                Initializetags();
                _tagsInitialized = true;
            }

            if (!_layersInitialized)
            {
                InitializeLayers();
                _layersInitialized = true;
            }
        }

        private void InitializeLayers()
        {
#if UNITY_EDITOR
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = tagManager.FindProperty("layers");

            bool foundLayer = false;
            foreach (var layer in _layersNeeded)
            {
                for (int i = 0; i < layersProp.arraySize; i++)
                {
                    SerializedProperty t = layersProp.GetArrayElementAtIndex(i);
                    if (t.stringValue.Equals(layer)) { foundLayer = true; break; }
                }

                if (!foundLayer)
                {
                    SerializedProperty slot = null;
                    for (int i = 8; i <= 31; i++)
                    {
                        SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
                        if (sp != null && string.IsNullOrEmpty(sp.stringValue))
                        {
                            slot = sp;
                            break;
                        }
                    }

                    if (slot != null)
                    {
                        slot.stringValue = layer;
                    }
                    else
                    {
                        Debug.LogError("Could not find an open Layer Slot for: " + name);
                    }

                }
                foundLayer = true;
            }

            tagManager.ApplyModifiedProperties();

            if (GameObject.FindGameObjectWithTag("MapSO")!=null&&foundLayer)
            {
                GameObject.FindGameObjectWithTag("MapSO").layer =LayerMask.NameToLayer("Collisions");
            }
#endif
        }

            private void Initializetags()
        {
#if UNITY_EDITOR
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            bool foundTag = false;
            foreach (var tag in _tagsNeeded)
            {
                for (int i = 0; i < tagsProp.arraySize; i++)
                {
                    SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                    if (t.stringValue.Equals(tag)) { foundTag = true; break; }
                }

                if (!foundTag)
                {
                    tagsProp.InsertArrayElementAtIndex(0);
                    SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                    n.stringValue = tag;
                }
                foundTag = true;
            }

            tagManager.ApplyModifiedProperties();

            if (GameObject.FindGameObjectsWithTag("SpawnHolder").Length == 0)
            {
                GameObject SpawnedColliderHolder = new GameObject("SpawnHolder");
                SpawnedColliderHolder.transform.tag = "SpawnHolder";
            }

            if (GameObject.FindGameObjectsWithTag("SpawnerFreeCellHolder").Length == 0)
            {
                GameObject SpawnedFreeCellHolder = new GameObject("SpawnerFreeCellHolder");
                SpawnedFreeCellHolder.transform.tag = "SpawnerFreeCellHolder";
            }
#endif
        }

        void OnValuesUpdated()
        {
            if (_mapSOLoadFrom != null && this != null)
            {
                LoadMap();
            }
        }

        void OnValidate()
        {
            if (_mapSOLoadFrom!=null)
            {
                _mapSOLoadFrom.OnValuesUpdated -= OnValuesUpdated;

                if (_mapSOLoadFrom != null)
                {
                    _mapSOLoadFrom.OnValuesUpdated += OnValuesUpdated;
                }
            }
        }

        void ActualizeMapBoundsSize()
        {
            _mapBounds.GetComponent<BoxCollider>().size = new Vector3(_map._width, _map._height, 1) * _map._squareSize;
        }

        public void GenerateMap(bool inEditMode)
        {
            if (inEditMode)
            {
                return;
            }
            ActualizeMapBoundsSize();
            _mapArray = new int[_map._width, _map._height];
            RandomFillMap();

            for (int i = 0; i < _map._smoothMapIterations; i++)
            {
                SmoothMap();
            }

            ProcessMap();

            int[,] borderedMap = new int[_map._width + _borderMap * 2, _map._height + _borderMap * 2];

            for (int x = 0; x < borderedMap.GetLength(0); x++)
            {
                for (int y = 0; y < borderedMap.GetLength(1); y++)
                {
                    if (x >= _borderMap && x < _map._width + _borderMap && y >= _borderMap && y < _map._height + _borderMap)
                    {
                        borderedMap[x, y] = _mapArray[x - _borderMap, y - _borderMap];
                    }
                    else
                    {
                        borderedMap[x, y] = 1;
                    }
                }
            }

            _meshGen.GenerateMesh(borderedMap, _map._squareSize);

            SpawnFreeCell(_spawnFreeCellBool);

            if (_mapMesh != null)
            {
                _mapMesh.GetComponent<Renderer>().material = _map._mapMeshMat;
            }

            if (_mapBackground != null)
            {
                _mapBackground.GetComponent<Renderer>().material = _map._backgroundMat;
            }

        }

        public void ClearAllFreeCellSpawnedObjects()
        {

            _spawnedFreeCellHolder = GameObject.FindGameObjectWithTag("SpawnerFreeCellHolder");

            while (_spawnedFreeCellHolder.transform.childCount > 0)
            {
                DestroyImmediate(_spawnedFreeCellHolder.transform.GetChild(0).gameObject);
            }

        }

        public void SpawnFreeCell(bool _spawnFreeCellBool)
        {
            _freeCellSpawnersCount = 0;
            if (_spawnFreeCellBool)
            {
                ClearAllFreeCellSpawnedObjects();

                if (_map._spawnerFreeCell != null)
                {
                    foreach (var test in _map._spawnerFreeCell)
                    {
                        SpawnerInFreeMapCells t = test;
                        if (t != null)
                        {
                            t.Init();
                            Spawn(t);
                            _freeCellSpawnersCount++;
                        }
                    }
                }
            }
        }

        void ProcessMap()
        {
            List<List<Coord>> wallRegions = GetRegions(1);
            int wallThresholdSize = 50;

            foreach (List<Coord> wallRegion in wallRegions)
            {
                if (wallRegion.Count < wallThresholdSize)
                {
                    foreach (Coord tile in wallRegion)
                    {
                        _mapArray[tile.tileX, tile.tileY] = 0;
                    }
                }
            }

            List<List<Coord>> roomRegions = GetRegions(0);
            int roomThresholdSize = 50;
            List<Room> survivingRooms = new List<Room>();

            foreach (List<Coord> roomRegion in roomRegions)
            {
                if (roomRegion.Count < roomThresholdSize)
                {
                    foreach (Coord tile in roomRegion)
                    {
                        _mapArray[tile.tileX, tile.tileY] = 1;
                    }
                }
                else
                {
                    survivingRooms.Add(new Room(roomRegion, _mapArray));
                }
            }
            survivingRooms.Sort();
            if (survivingRooms != null && survivingRooms.Count > 0)
            {
                survivingRooms[0].isMainRoom = true;
                survivingRooms[0].isAccessibleFromMainRoom = true;
            }

            _freepos = GetRegions(0);

            ConnectClosestRooms(survivingRooms);

            //find viable pos to spawn player
            _posToSpawnPlayer = GameObject.FindGameObjectWithTag("SpawnPlayerPos") ? GameObject.FindGameObjectWithTag("SpawnPlayerPos") : null;

            if (_posToSpawnPlayer != null)
            {
                Coord _playerSpawnCoord = WorldPointToCoord(_posToSpawnPlayer.transform.localPosition / _map._squareSize);
                bool isSpawnablePos = spawnablePos(_playerSpawnCoord);


                while (!isSpawnablePos)
                {
                    int index = UnityEngine.Random.Range(0, _freepos.Count);
                    List<Coord> coords = _freepos[index];
                    int index2 = UnityEngine.Random.Range(0, coords.Count);
                    Vector3 newSpawnPos = CoordToWorldPoint(coords[index2]);
                    _posToSpawnPlayer.transform.localPosition = newSpawnPos;
                    _playerSpawnCoord = WorldPointToCoord(_posToSpawnPlayer.transform.localPosition / _map._squareSize);
                    isSpawnablePos = spawnablePos(_playerSpawnCoord);
                }
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player") ? GameObject.FindGameObjectWithTag("Player") : null;
            if (player!=null)
            {
                player.GetComponent<Player>().SetPos();
            }
            //.........................
        }

        void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false)
        {

            List<Room> roomListA = new List<Room>();
            List<Room> roomListB = new List<Room>();

            if (forceAccessibilityFromMainRoom)
            {
                foreach (Room room in allRooms)
                {
                    if (room.isAccessibleFromMainRoom)
                    {
                        roomListB.Add(room);
                    }
                    else
                    {
                        roomListA.Add(room);
                    }
                }
            }
            else
            {
                roomListA = allRooms;
                roomListB = allRooms;
            }

            int bestDistance = 0;
            Coord bestTileA = new Coord();
            Coord bestTileB = new Coord();
            Room bestRoomA = new Room();
            Room bestRoomB = new Room();
            bool possibleConnectionFound = false;

            foreach (Room roomA in roomListA)
            {
                if (!forceAccessibilityFromMainRoom)
                {
                    possibleConnectionFound = false;
                    if (roomA.connectedRooms.Count > 0)
                    {
                        continue;
                    }
                }

                foreach (Room roomB in roomListB)
                {
                    if (roomA == roomB || roomA.IsConnected(roomB))
                    {
                        continue;
                    }

                    for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                    {
                        for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                        {
                            Coord tileA = roomA.edgeTiles[tileIndexA];
                            Coord tileB = roomB.edgeTiles[tileIndexB];
                            int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                            if (distanceBetweenRooms < bestDistance || !possibleConnectionFound)
                            {
                                bestDistance = distanceBetweenRooms;
                                possibleConnectionFound = true;
                                bestTileA = tileA;
                                bestTileB = tileB;
                                bestRoomA = roomA;
                                bestRoomB = roomB;
                            }
                        }
                    }
                }
                if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
                {
                    CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                }
            }

            if (possibleConnectionFound && forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                ConnectClosestRooms(allRooms, true);
            }

            if (!forceAccessibilityFromMainRoom)
            {
                ConnectClosestRooms(allRooms, true);
            }
        }

        void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
        {
            Room.ConnectRooms(roomA, roomB);
            //Debug.DrawLine (CoordToWorldPoint (tileA), CoordToWorldPoint (tileB), Color.green, 100);

            List<Coord> line = GetLine(tileA, tileB);
            foreach (Coord c in line)
            {
                DrawCircle(c, _map._tunnelRadio);
            }
        }

        void DrawCircle(Coord c, int r)
        {
            for (int x = -r; x <= r; x++)
            {
                for (int y = -r; y <= r; y++)
                {
                    if (x * x + y * y <= r * r)
                    {
                        int drawX = c.tileX + x;
                        int drawY = c.tileY + y;
                        if (IsInMapRange(drawX, drawY))
                        {
                            _mapArray[drawX, drawY] = 0;
                        }
                    }
                }
            }
        }

        List<Coord> GetLine(Coord from, Coord to)
        {
            List<Coord> line = new List<Coord>();

            int x = from.tileX;
            int y = from.tileY;

            int dx = to.tileX - from.tileX;
            int dy = to.tileY - from.tileY;

            bool inverted = false;
            int step = Math.Sign(dx);
            int gradientStep = Math.Sign(dy);

            int longest = Mathf.Abs(dx);
            int shortest = Mathf.Abs(dy);

            if (longest < shortest)
            {
                inverted = true;
                longest = Mathf.Abs(dy);
                shortest = Mathf.Abs(dx);

                step = Math.Sign(dy);
                gradientStep = Math.Sign(dx);
            }

            int gradientAccumulation = longest / 2;
            for (int i = 0; i < longest; i++)
            {
                line.Add(new Coord(x, y));

                if (inverted)
                {
                    y += step;
                }
                else
                {
                    x += step;
                }

                gradientAccumulation += shortest;
                if (gradientAccumulation >= longest)
                {
                    if (inverted)
                    {
                        x += gradientStep;
                    }
                    else
                    {
                        y += gradientStep;
                    }
                    gradientAccumulation -= longest;
                }
            }

            return line;
        }

        Vector2 CoordToWorldPoint(Coord tile)
        {
            return new Vector2(-_map._width / 2 + .5f + tile.tileX, -_map._height / 2 + .5f + tile.tileY) * _map._squareSize;
        }

        //...........
        public Coord WorldPointToCoord(Vector2 worldPoint)
        {
            Coord coord = new Coord(Mathf.RoundToInt(worldPoint.x + _map._width / 2 - .5f), Mathf.RoundToInt(worldPoint.y + _map._height / 2 - .5f));
            return coord;
        }

        public bool spawnablePos(Coord coord)
        {
            if (_freepos != null)
            {
                foreach (List<Coord> list in _freepos)
                {
                    if (list.Contains(coord))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        //..............................test
        void Spawn(SpawnerInFreeMapCells test)
        {
            if (test != null)
            {
                List<Vector2> puntos = test.points;
                List<Vector2> copyPoints = new List<Vector2>();
                foreach (var point in puntos)
                {
                    copyPoints.Add(point - test.regionSize / 2);
                }
                puntos.Clear();
                foreach (var point in copyPoints)
                {
                    if (spawnablePos(WorldPointToCoord(point / _map._squareSize)))
                    {
                        puntos.Add(point);
                    }
                }
                test.Spawn(puntos);
            }
        }
        //.............
        List<List<Coord>> GetRegions(int tileType)
        {
            List<List<Coord>> regions = new List<List<Coord>>();
            int[,] mapFlags = new int[_map._width, _map._height];

            for (int x = 0; x < _map._width; x++)
            {
                for (int y = 0; y < _map._height; y++)
                {
                    if (mapFlags[x, y] == 0 && _mapArray[x, y] == tileType)
                    {
                        List<Coord> newRegion = GetRegionTiles(x, y);
                        regions.Add(newRegion);

                        foreach (Coord tile in newRegion)
                        {
                            mapFlags[tile.tileX, tile.tileY] = 1;
                        }
                    }
                }
            }

            return regions;
        }

        List<Coord> GetRegionTiles(int startX, int startY)
        {
            List<Coord> tiles = new List<Coord>();
            int[,] mapFlags = new int[_map._width, _map._height];
            int tileType = _mapArray[startX, startY];

            Queue<Coord> queue = new Queue<Coord>();
            queue.Enqueue(new Coord(startX, startY));
            mapFlags[startX, startY] = 1;

            while (queue.Count > 0)
            {
                Coord tile = queue.Dequeue();
                tiles.Add(tile);

                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                {
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                    {
                        if (IsInMapRange(x, y) && (y == tile.tileY || x == tile.tileX))
                        {
                            if (mapFlags[x, y] == 0 && _mapArray[x, y] == tileType)
                            {
                                mapFlags[x, y] = 1;
                                queue.Enqueue(new Coord(x, y));
                            }
                        }
                    }
                }
            }
            return tiles;
        }

        bool IsInMapRange(int x, int y)
        {
            return x >= 0 && x < _map._width && y >= 0 && y < _map._height;
        }


        void RandomFillMap()
        {
            System.Random pseudoRandom = new System.Random(_map._seed.GetHashCode());

            for (int x = 0; x < _map._width; x++)
            {
                for (int y = 0; y < _map._height; y++)
                {
                    if (x == 0 || x == _map._width - 1 || y == 0 || y == _map._height - 1)
                    {
                        _mapArray[x, y] = 1;
                    }
                    else
                    {
                        _mapArray[x, y] = pseudoRandom.Next(0, 100) < _map._randomFillPercent ? 1 : 0;
                    }
                }
            }
        }

        void SmoothMap()
        {
            for (int x = 0; x < _map._width; x++)
            {
                for (int y = 0; y < _map._height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                        _mapArray[x, y] = 1;
                    else if (neighbourWallTiles < 4)
                        _mapArray[x, y] = 0;

                }
            }
        }

        int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (IsInMapRange(neighbourX, neighbourY))
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += _mapArray[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }

        public struct Coord
        {
            public int tileX;
            public int tileY;

            public Coord(int x, int y)
            {
                tileX = x;
                tileY = y;
            }
        }


        class Room : IComparable<Room>
        {
            public List<Coord> tiles;
            public List<Coord> edgeTiles;
            public List<Room> connectedRooms;
            public int roomSize;
            public bool isAccessibleFromMainRoom;
            public bool isMainRoom;

            public Room()
            {
            }

            public Room(List<Coord> roomTiles, int[,] map)
            {
                tiles = roomTiles;
                roomSize = tiles.Count;
                connectedRooms = new List<Room>();

                edgeTiles = new List<Coord>();
                foreach (Coord tile in tiles)
                {
                    for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                    {
                        for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                        {
                            if (x == tile.tileX || y == tile.tileY)
                            {
                                if (map[x, y] == 1)
                                {
                                    edgeTiles.Add(tile);
                                }
                            }
                        }
                    }
                }
            }

            public void SetAccessibleFromMainRoom()
            {
                if (!isAccessibleFromMainRoom)
                {
                    isAccessibleFromMainRoom = true;
                    foreach (Room connectedRoom in connectedRooms)
                    {
                        connectedRoom.SetAccessibleFromMainRoom();
                    }
                }
            }

            public static void ConnectRooms(Room roomA, Room roomB)
            {
                if (roomA.isAccessibleFromMainRoom)
                {
                    roomB.SetAccessibleFromMainRoom();
                }
                else if (roomB.isAccessibleFromMainRoom)
                {
                    roomA.SetAccessibleFromMainRoom();
                }
                roomA.connectedRooms.Add(roomB);
                roomB.connectedRooms.Add(roomA);
            }

            public bool IsConnected(Room otherRoom)
            {
                return connectedRooms.Contains(otherRoom);
            }

            public int CompareTo(Room otherRoom)
            {
                return otherRoom.roomSize.CompareTo(roomSize);
            }
        }

    }
}