using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace CNB
{
    public class MeshGeneratorCNB : MonoBehaviour
    {
        /// <summary>
        /// Generates a new mesh when demanded from "MapGeneratorCNB.cs". Also creates one LineRenderer components for every outline of the map, and sets their points base on the Bezier curves and vertex data calculetes in respective clases.
        /// If _mapGen._mapSOLoadFrom.inEditMode==true, saves the needed data to the scriptable object (_mapGen._mapSOLoadFrom) for editing the mesh and regenerating the colliders and Line Renderer components
        /// Genera una nueva malla cuando se solicita desde "MapGeneratorCNB.cs". También crea un componente LineRenderer para cada contorno del mapa, y establece sus puntos en base a las curvas Bezier y cálculos de datos de vértice en las clases respectivas.
        /// Si _mapGen._mapSOLoadFrom.inEditMode==true, guarda los datos necesarios en el scriptable object (_mapGen._mapSOLoadFrom) para editar la malla y regenerar los colisionadores y los componentes Line Renderer
        /// 当“MapGeneratorCNB.cs”请求时生成新的网格。它还为地图上的每个轮廓创建一个 LineRenderer 组件，并根据相应类中的贝塞尔曲线和顶点数据计算来设置其点。
        /// 如果 _mapGen._mapSOLoadFrom.inEditMode==true，则将必要的数据存储在可编写脚本的对象 (_mapGen._mapSOLoadFrom) 中以编辑网格并重新生成碰撞器和线渲染器组件
        /// </summary>

        List<BezierPath> paths = new List<BezierPath>();
        List<VertexPath> vertexPaths = new List<VertexPath>();
        [HideInInspector]
        List<GameObject> lineRenderers = new List<GameObject>();
        List<EdgeCollider2D> edgeColliders = new List<EdgeCollider2D>();
        [HideInInspector]
        public List<Material> _lineRendMat;
        [HideInInspector]
        public MapGeneratorCNB _mapGen;
        Mesh mesh;
        //............................................

        [HideInInspector]
        public SquareGrid squareGrid;
        [HideInInspector]
        public MeshFilter cave;
        [HideInInspector]
        public List<Vector3> vertices = new List<Vector3>();
        [HideInInspector]
        public List<int> triangles = new List<int>();
        Dictionary<int, List<Triangle>> triangleDictionary = new Dictionary<int, List<Triangle>>();
        [HideInInspector]
        public List<List<int>> outlines = new List<List<int>>();
        HashSet<int> checkedVertices = new HashSet<int>();

        // spawn
        List<Vector2[]> allPoints = new List<Vector2[]>();
        Vector3[] allLocalNormals;


        [HideInInspector]
        public int tileAmount = 10;

        [HideInInspector]
        public SpawnerInMapSurfaces[] _spawnerCollider;
        [HideInInspector]
        public int lineRendererTextureTilling = 25;

        public bool _spawnInCollidersBool = false;
        //reduce mesh density
        int indexTempAmount = 0;
        int squareCount = 0;

        public List<List<int>> outlinesBU;

        //edit mesh helper
        [Range(0f, 4f)]
        public float editGroupDistance = 1;

        [Range(0f, 1f)]
        [HideInInspector]
        public float lineRendererVertexSpacing = .1f;

        //............................................
        [HideInInspector]
        public int _colliderSpawnersCount = 0;
        [HideInInspector]
        public int _colliderSpawnedGOGlobalCount = 0;


        public void EnterEditModeCreateNewMeshAndSaveData()
        {
            _mapGen._mapSOLoadFrom.verticesBackUp = vertices;
            _mapGen._mapSOLoadFrom.trianglesBackUp = triangles;
            Mesh meshBU = new Mesh();
            meshBU.vertices = _mapGen._mapSOLoadFrom.verticesBackUp.ToArray();
            meshBU.triangles = _mapGen._mapSOLoadFrom.trianglesBackUp.ToArray();
            int width = _mapGen._mapSOLoadFrom._map.width;
            int height = _mapGen._mapSOLoadFrom._map.height;
            int squareSize = _mapGen._mapSOLoadFrom._map.squareSize;
            int borderMap = _mapGen._borderMap;
            Vector2[] uvs = new Vector2[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                float percentX = Mathf.InverseLerp(-(width + 2 * borderMap) / 2 * squareSize, (width + 2 * borderMap) / 2 * squareSize, vertices[i].x) * tileAmount;
                float percentY = Mathf.InverseLerp(-(height + 2 * borderMap) / 2 * squareSize, (height + 2 * borderMap) / 2 * squareSize, vertices[i].y) * tileAmount;
                uvs[i] = new Vector2(percentX, percentY);
            }
            _mapGen._mapSOLoadFrom.uvsBackUp = new List<Vector2>(uvs);
            meshBU.uv = uvs;
            meshBU.RecalculateNormals();
            cave.mesh = meshBU;
            cave.GetComponent<MeshRenderer>().material = _mapGen._mapSOLoadFrom._mapMatSO._meshMaterial;
            _mapGen._mapSOLoadFrom.inEditMode = true;
        }

        public void CleanMap()
        {
            edgeColliders = new List<EdgeCollider2D>(GetComponents<EdgeCollider2D>() != null ? GetComponents<EdgeCollider2D>() : null);
            foreach (var edgeCol in edgeColliders)
            {
                DestroyImmediate(edgeCol, true);
            }
            edgeColliders.Clear();

            lineRenderers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Line Renderer"));
            foreach (var lineRenderer in lineRenderers)
            {
                DestroyImmediate(lineRenderer, true);
            }
            _lineRendMat.Clear();
            lineRenderers.Clear();
        }

        public void GenerateMesh(int[,] map, float squareSize)
        {
            indexTempAmount = 0;
            squareCount = 0;

            triangleDictionary.Clear();
            outlines.Clear();
            checkedVertices.Clear();

            squareGrid = new SquareGrid(map, squareSize);

            vertices.Clear();
            triangles.Clear();

            AssignVertexForMesh();

            mesh = new Mesh();
            cave.mesh = mesh;

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            Vector2[] uvs = new Vector2[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                float percentX = Mathf.InverseLerp(-map.GetLength(0) / 2 * squareSize, map.GetLength(0) / 2 * squareSize, vertices[i].x) * tileAmount;
                float percentY = Mathf.InverseLerp(-map.GetLength(1) / 2 * squareSize, map.GetLength(1) / 2 * squareSize, vertices[i].y) * tileAmount;
                uvs[i] = new Vector2(percentX, percentY);
            }
            mesh.uv = uvs;

            Generate2DColliders(vertices, _mapGen._mapSOLoadFrom.inEditMode);
        }

        private void AssignVertexForMesh()
        {
            for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                {
                    TriangulateSquare(squareGrid.squares[x, y], x, y);
                }
            }
        }

        public void Generate2DColliders(List<Vector3> vertices, bool editMode)
        {
            //mio...................................................................aa
            if (editMode)
            {
                vertices = _mapGen._mapSOLoadFrom.verticesBackUp;
                triangles = _mapGen._mapSOLoadFrom.trianglesBackUp;
                outlines.Clear();

                #region SOserialization problem with lists of lists
                outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo0);
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo1?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo1);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo2?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo2);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo3?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo3);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo4?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo4);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo5?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo5);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo6?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo6);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo7?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo7);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo8?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo8);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo9?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo9);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo10?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo10);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo11?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo11);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo12?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo12);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo13?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo13);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo14?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo14);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo15?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo15);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo16?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo16);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo17?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo17);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo18?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo18);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo19?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo19);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo20?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo20);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo21?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo21);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo22?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo22);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo23?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo23);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo24?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo24);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo25?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo25);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo26?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo26);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo27?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo27);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo28?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo28);

                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo29?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo29);
                }
                if (_mapGen._mapSOLoadFrom.outlinesBackUpInfo30?.Count > 0)
                {
                    outlines.Add(_mapGen._mapSOLoadFrom.outlinesBackUpInfo30);
                }
                #endregion
            }

            CleanMap();
            vertexPaths.Clear();
            paths.Clear();

            if (!editMode)
            {
                CalculateMeshOutlines(vertices);
            }

            //mio.................................................................................
            foreach (List<int> outline in outlines)
            {
                EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
                edgeCollider.edgeRadius = .1f;
                edgeColliders.Add(edgeCollider);

                GameObject newLineRenderer = new GameObject("lineRendererObject");
                newLineRenderer.tag = "Line Renderer";
                newLineRenderer.AddComponent<LineRenderer>();
                lineRenderers.Add(newLineRenderer);

                Vector2[] edgePoints = new Vector2[outline.Count];

                for (int i = 0; i < outline.Count; i++)
                {
                    edgePoints[i] = new Vector2(vertices[outline[i]].x, vertices[outline[i]].y);
                }

                allPoints.Add(edgePoints);
                edgeCollider.points = edgePoints;
                BezierPath path = new BezierPath(edgePoints);
                paths.Add(path);
                VertexPath vertexPath = new VertexPath(path, transform, lineRendererVertexSpacing);
                vertexPaths.Add(vertexPath);

                allLocalNormals = vertexPath.localNormals;

                LineRenderer rendererComponent = newLineRenderer.GetComponent<LineRenderer>();
                if (_mapGen != null && _mapGen._map._mapLineRendMat != null)
                {
                    _lineRendMat.Add(Instantiate(_mapGen._map._mapLineRendMat));
                    rendererComponent.positionCount = vertexPath.NumPoints;
                    rendererComponent.loop = true;
                    rendererComponent.sharedMaterial = _lineRendMat[_lineRendMat.Count - 1];
                    rendererComponent.sharedMaterial.mainTextureScale = new Vector2(rendererComponent.positionCount / lineRendererTextureTilling, 1);
                }

                for (int j = 0; j < rendererComponent.positionCount; j++)
                {
                    Vector3 pos = vertexPath.localPoints[j];
                    rendererComponent.SetPosition(j, pos);
                }

                if (!editMode)
                {
                    SpawnInColliders(_spawnInCollidersBool);
                }

                //.......................................................................
            }
        }

        public void SpawnInColliders(bool genColl)
        {
            _colliderSpawnersCount = 0;
            _colliderSpawnedGOGlobalCount = 0;
            if (genColl)
            {
                ClearAllColliderSpawnedObjects();

                if (_spawnerCollider != null && _spawnerCollider.Length > 0)
                {
                    foreach (var p in _spawnerCollider)
                    {
                        if (p != null && vertexPaths != null && vertexPaths.Count > 0)
                        {
                            _colliderSpawnersCount++;
                            p.Generate(gameObject, vertexPaths.ToArray(), genColl);
                        }
                    }
                }
            }
        }

        public void ClearAllColliderSpawnedObjects()
        {
            GameObject _spawnedCollHolder = GameObject.FindGameObjectWithTag("SpawnHolder");

            if (_spawnedCollHolder != null)
            {
                while (_spawnedCollHolder.transform.childCount > 0)
                {
                    DestroyImmediate(_spawnedCollHolder.transform.GetChild(0).gameObject);
                }
            }
        }

        void TriangulateSquare(Square square, int indexX, int indexY)
        {

            bool firstSqareInRow = indexY == 0;
            bool lastSquareInRow = indexY == squareGrid.squares.GetLength(1) - 1;

            Square squareAtPrecedentIndex = lastSquareInRow || firstSqareInRow ? squareGrid.squares[indexX, indexY] : squareGrid.squares[indexX, indexY - 1]; ;
            Square firstSquareToReduceMeshRes = !firstSqareInRow ? squareGrid.squares[indexX, indexY - indexTempAmount] : squareGrid.squares[indexX, indexY];

            if (squareAtPrecedentIndex.configuration == 15 && square.configuration != 15 || lastSquareInRow)
            {
                MeshFromPoints(squareAtPrecedentIndex.topLeft, squareAtPrecedentIndex.topRight, firstSquareToReduceMeshRes.bottomRight, firstSquareToReduceMeshRes.bottomLeft);
                checkedVertices.Add(squareAtPrecedentIndex.topLeft.vertexIndex);
                checkedVertices.Add(squareAtPrecedentIndex.topRight.vertexIndex);
                checkedVertices.Add(firstSquareToReduceMeshRes.bottomRight.vertexIndex);
                checkedVertices.Add(firstSquareToReduceMeshRes.bottomLeft.vertexIndex);
                indexTempAmount = lastSquareInRow ? -1 : 0;
            }

            switch (square.configuration)
            {
                case 0:
                    break;

                // 1 points:
                case 1:
                    MeshFromPoints(square.centreLeft, square.centreBottom, square.bottomLeft);
                    break;
                case 2:
                    MeshFromPoints(square.bottomRight, square.centreBottom, square.centreRight);
                    break;
                case 4:
                    MeshFromPoints(square.topRight, square.centreRight, square.centreTop);
                    break;
                case 8:
                    MeshFromPoints(square.topLeft, square.centreTop, square.centreLeft);
                    break;

                // 2 points:
                case 3:
                    MeshFromPoints(square.centreRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                    break;
                case 6:
                    MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.centreBottom);
                    break;
                case 9:
                    MeshFromPoints(square.topLeft, square.centreTop, square.centreBottom, square.bottomLeft);
                    checkedVertices.Add(square.topLeft.vertexIndex);
                    checkedVertices.Add(square.bottomLeft.vertexIndex);
                    break;
                case 12:
                    MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreLeft);
                    break;
                case 5:
                    MeshFromPoints(square.centreTop, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft, square.centreLeft);
                    break;
                case 10:
                    MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.centreBottom, square.centreLeft);
                    break;

                // 3 point:
                case 7:
                    MeshFromPoints(square.centreTop, square.topRight, square.bottomRight, square.bottomLeft, square.centreLeft);
                    break;
                case 11:
                    MeshFromPoints(square.topLeft, square.centreTop, square.centreRight, square.bottomRight, square.bottomLeft);
                    break;
                case 13:
                    MeshFromPoints(square.topLeft, square.topRight, square.centreRight, square.centreBottom, square.bottomLeft);
                    break;
                case 14:
                    MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.centreBottom, square.centreLeft);
                    break;

                // 4 point:
                case 15:
                    //MeshFromPoints(square.topLeft, square.topRight, square.bottomRight, square.bottomLeft);
                    checkedVertices.Add(square.topLeft.vertexIndex);
                    checkedVertices.Add(square.topRight.vertexIndex);
                    checkedVertices.Add(square.bottomRight.vertexIndex);
                    checkedVertices.Add(square.bottomLeft.vertexIndex);
                    indexTempAmount++;
                    break;
            }
            squareCount++;
        }

        void MeshFromPoints(params Node[] points)
        {
            AssignVertices(points);

            if (points.Length >= 3)
                CreateTriangle(points[0], points[1], points[2]);
            if (points.Length >= 4)
                CreateTriangle(points[0], points[2], points[3]);
            if (points.Length >= 5)
                CreateTriangle(points[0], points[3], points[4]);
            if (points.Length >= 6)
                CreateTriangle(points[0], points[4], points[5]);

        }

        void AssignVertices(Node[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].vertexIndex == -1)
                {
                    points[i].vertexIndex = vertices.Count;
                    vertices.Add(points[i].position);
                }
            }
        }

        void CreateTriangle(Node a, Node b, Node c)
        {
            triangles.Add(a.vertexIndex);
            triangles.Add(b.vertexIndex);
            triangles.Add(c.vertexIndex);

            Triangle triangle = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);
            AddTriangleToDictionary(triangle.vertexIndexA, triangle);
            AddTriangleToDictionary(triangle.vertexIndexB, triangle);
            AddTriangleToDictionary(triangle.vertexIndexC, triangle);
        }

        void AddTriangleToDictionary(int vertexIndexKey, Triangle triangle)
        {
            if (triangleDictionary.ContainsKey(vertexIndexKey))
            {
                triangleDictionary[vertexIndexKey].Add(triangle);
            }
            else
            {
                List<Triangle> triangleList = new List<Triangle>();
                triangleList.Add(triangle);
                triangleDictionary.Add(vertexIndexKey, triangleList);
            }
        }

        void CalculateMeshOutlines(List<Vector3> vertices)
        {

            for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
            {
                if (!checkedVertices.Contains(vertexIndex))
                {
                    int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);
                    if (newOutlineVertex != -1)
                    {
                        checkedVertices.Add(vertexIndex);

                        List<int> newOutline = new List<int>();
                        newOutline.Add(vertexIndex);
                        outlines.Add(newOutline);
                        FollowOutline(newOutlineVertex, outlines.Count - 1);
                        outlines[outlines.Count - 1].Add(vertexIndex);
                    }
                }
            }

            #region SOserialization problem with lists of lists
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo0 = outlines[0];
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo1 = outlines.Count > 1 ? outlines[1] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo2 = outlines.Count > 2 ? outlines[2] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo3 = outlines.Count > 3 ? outlines[3] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo4 = outlines.Count > 4 ? outlines[4] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo5 = outlines.Count > 5 ? outlines[5] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo6 = outlines.Count > 6 ? outlines[6] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo7 = outlines.Count > 7 ? outlines[7] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo8 = outlines.Count > 8 ? outlines[8] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo9 = outlines.Count > 9 ? outlines[9] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo10 = outlines.Count > 10 ? outlines[10] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo11 = outlines.Count > 11 ? outlines[11] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo12 = outlines.Count > 12 ? outlines[12] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo13 = outlines.Count > 13 ? outlines[13] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo14 = outlines.Count > 14 ? outlines[14] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo15 = outlines.Count > 15 ? outlines[15] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo16 = outlines.Count > 16 ? outlines[16] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo17 = outlines.Count > 17 ? outlines[17] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo18 = outlines.Count > 18 ? outlines[18] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo19 = outlines.Count > 19 ? outlines[19] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo20 = outlines.Count > 20 ? outlines[20] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo21 = outlines.Count > 21 ? outlines[21] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo22 = outlines.Count > 22 ? outlines[22] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo23 = outlines.Count > 23 ? outlines[23] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo24 = outlines.Count > 24 ? outlines[24] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo25 = outlines.Count > 25 ? outlines[25] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo26 = outlines.Count > 26 ? outlines[26] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo27 = outlines.Count > 27 ? outlines[27] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo28 = outlines.Count > 28 ? outlines[28] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo29 = outlines.Count > 29 ? outlines[29] : null;
            _mapGen._mapSOLoadFrom.outlinesBackUpInfo30 = outlines.Count > 30 ? outlines[30] : null;
            #endregion

        }

        void FollowOutline(int vertexIndex, int outlineIndex)
        {
            outlines[outlineIndex].Add(vertexIndex);
            checkedVertices.Add(vertexIndex);
            int nextVertexIndex = GetConnectedOutlineVertex(vertexIndex);

            if (nextVertexIndex != -1)
            {
                FollowOutline(nextVertexIndex, outlineIndex);
            }
        }

        int GetConnectedOutlineVertex(int vertexIndex)
        {
            List<Triangle> trianglesContainingVertex = triangleDictionary[vertexIndex];

            for (int i = 0; i < trianglesContainingVertex.Count; i++)
            {
                Triangle triangle = trianglesContainingVertex[i];

                for (int j = 0; j < 3; j++)
                {
                    int vertexB = triangle[j];
                    if (vertexB != vertexIndex && !checkedVertices.Contains(vertexB))
                    {
                        if (IsOutlineEdge(vertexIndex, vertexB))
                        {
                            return vertexB;
                        }
                    }
                }
            }

            return -1;
        }

        bool IsOutlineEdge(int vertexA, int vertexB)
        {
            List<Triangle> trianglesContainingVertexA = triangleDictionary[vertexA];
            int sharedTriangleCount = 0;

            for (int i = 0; i < trianglesContainingVertexA.Count; i++)
            {
                if (trianglesContainingVertexA[i].Contains(vertexB))
                {
                    sharedTriangleCount++;
                    if (sharedTriangleCount > 1)
                    {
                        break;
                    }
                }
            }
            return sharedTriangleCount == 1;
        }

        struct Triangle
        {
            public int vertexIndexA;
            public int vertexIndexB;
            public int vertexIndexC;
            int[] vertices;

            public Triangle(int a, int b, int c)
            {
                vertexIndexA = a;
                vertexIndexB = b;
                vertexIndexC = c;

                vertices = new int[3];
                vertices[0] = a;
                vertices[1] = b;
                vertices[2] = c;
            }

            public int this[int i]
            {
                get
                {
                    return vertices[i];
                }
            }


            public bool Contains(int vertexIndex)
            {
                return vertexIndex == vertexIndexA || vertexIndex == vertexIndexB || vertexIndex == vertexIndexC;
            }
        }

        public class SquareGrid
        {
            public Square[,] squares;

            public SquareGrid(int[,] map, float squareSize)
            {
                int nodeCountX = map.GetLength(0);
                int nodeCountY = map.GetLength(1);
                float mapWidth = nodeCountX * squareSize;
                float mapHeight = nodeCountY * squareSize;

                ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

                for (int x = 0; x < nodeCountX; x++)
                {
                    for (int y = 0; y < nodeCountY; y++)
                    {
                        Vector2 pos = new Vector2(-mapWidth / 2 + x * squareSize + squareSize / 2, -mapHeight / 2 + y * squareSize + squareSize / 2);
                        controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1, squareSize);
                    }
                }

                squares = new Square[nodeCountX - 1, nodeCountY - 1];
                for (int x = 0; x < nodeCountX - 1; x++)
                {
                    for (int y = 0; y < nodeCountY - 1; y++)
                    {
                        squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                    }
                }

            }
        }

        public class Square
        {

            public ControlNode topLeft, topRight, bottomRight, bottomLeft;
            public Node centreTop, centreRight, centreBottom, centreLeft;
            public int configuration;

            public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomRight, ControlNode _bottomLeft)
            {
                topLeft = _topLeft;
                topRight = _topRight;
                bottomRight = _bottomRight;
                bottomLeft = _bottomLeft;

                centreTop = topLeft.right;
                centreRight = bottomRight.above;
                centreBottom = bottomLeft.right;
                centreLeft = bottomLeft.above;

                if (topLeft.active)
                    configuration += 8;
                if (topRight.active)
                    configuration += 4;
                if (bottomRight.active)
                    configuration += 2;
                if (bottomLeft.active)
                    configuration += 1;
            }

        }

        public class Node
        {
            public Vector2 position;
            public int vertexIndex = -1;

            public Node(Vector2 _pos)
            {
                position = _pos;
            }
        }

        public class ControlNode : Node
        {

            public bool active;
            public Node above, right;

            public ControlNode(Vector2 _pos, bool _active, float squareSize) : base(_pos)
            {
                active = _active;
                above = new Node(position + Vector2.up * squareSize / 2f);
                right = new Node(position + Vector2.right * squareSize / 2f);
            }

        }
    }
}