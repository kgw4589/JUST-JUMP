using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CNB
{
    /// <summary>
    /// Enter and Exit EDIT MODE buttons. If EDIT MODE ==true use to edit the mesh modifiing mesh vertex positions with editor handles. After edited the mesh to the desired shape use the button "generate colliders". 
    /// If pussed button EXIT EDIT MODE, yout loose the edited changes, the map is regenerated to the state previous to enter editting mode and the funtionality to use the spawners is available again.
    /// Botones Entrar y Salir del MODO DE EDICIÓN. Si EDIT MODE ==true utilidades para definir la malla modificando las posiciones de los vértices de la malla con controladores de edición. Después de editar la malla a la forma deseada, use el botón "generar colisionadores".
    /// Si presiona el botón SALIR DEL MODO DE EDICIÓN, pierde los cambios editados, el mapa se regenera a la situación anterior a la edición y la funcionalidad para usar los generadores vuelve a estar disponible
    /// 进入和退出编辑模式按钮。如果 EDIT MODE ==true 实用程序通过使用编辑手柄修改网格顶点的位置来定义网格。将网格编辑为所需的形状后，使用“生成碰撞器”按钮。
    /// 如果按下 EXIT EDIT MODE 按钮，您将丢失编辑的更改，地图将重新生成为编辑前的情况，并且使用生成器的功能再次可用
    /// </summary>
    [CustomEditor(typeof(MeshGeneratorCNB))]
    public class MeshEditorCNB : Editor
    {

        MeshGeneratorCNB mesh;
        Vector2 mousePos;
        float editDistance = .3f;
        float drawOutlineDst = 6;

        List<Vector3> _vertices
        {
            get
            {
                return mesh.vertices;
            }
        }

        List<List<int>> _outlines
        {
            get
            {
                return mesh.outlines;
            }
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawProperties();
            serializedObject.ApplyModifiedProperties();
            SceneView.RepaintAll();
        }

        private void DrawProperties()
        {
            if (mesh != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("ENTER MESH EDIT MODE"))
                {
                    mesh.EnterEditModeCreateNewMeshAndSaveData();
                    EditorUtility.SetDirty(target);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                }
                if (GUILayout.Button("EXIT MESH EDIT MODE"))
                {
                    mesh._mapGen._mapSOLoadFrom.inEditMode = false;
                    mesh._mapGen.LoadMap();
                    EditorUtility.SetDirty(target);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                }
                EditorGUILayout.EndHorizontal();

                if (mesh._mapGen._mapSOLoadFrom!=null&&mesh._mapGen._mapSOLoadFrom.inEditMode == true)
                {
                    if (GUILayout.Button("GENERATE COLLIDERS"))
                    {
                        mesh.Generate2DColliders(_vertices, true);
                        EditorUtility.SetDirty(target);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                    }
                }
            }
            DrawDefaultInspector();
        }
        void OnSceneGUI()
        {
            if (mesh._mapGen._mapSOLoadFrom!=null&&mesh._mapGen._mapSOLoadFrom.inEditMode)
            {
                Input();
                Draw();
            }
        }

        void Input()
        {
            Event guiEvent = Event.current;
            mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        }

        void Draw()
        {
            Handles.color = new Color(1, 0, 0, .3f);
            Handles.DrawSolidDisc(mousePos, Vector3.forward, mesh.editGroupDistance);

            for (int i = 0; i < _outlines.Count; i++)
            {
                for (int j = 0; j < _outlines[i].Count; j++)
                {
                    float mouseDistToOutlineVertex = Vector2.Distance(_vertices[_outlines[i][j]], mousePos);
                    if (mouseDistToOutlineVertex < drawOutlineDst)
                    {
                        Handles.color = Color.red;
                        Handles.DrawLine(_vertices[_outlines[i][j]], _vertices[_outlines[i][(j + 1) % _outlines[i].Count]]);
                    }
                }
            }
            List<int> vertexIndexInEditRange = new List<int>();
            Vector3 deltaMove = Vector3.zero;
            int iterationIndex = 0;
            for (int k = 0; k < _vertices.Count; k++)
            {
                float mouseDistToVertex = Vector2.Distance(_vertices[k], mousePos);

                if (mouseDistToVertex < drawOutlineDst)
                {
                    Handles.color = Color.cyan;
                    Handles.DrawSolidDisc(_vertices[k], Vector3.forward, .1f);
                }
                if (mouseDistToVertex < mesh.editGroupDistance)
                {
                    vertexIndexInEditRange.Add(k);

                    if (mouseDistToVertex < editDistance)
                    {
                        Handles.color = Color.green;
                        EditorGUI.BeginChangeCheck();
                        Vector3 freeHandle = Handles.FreeMoveHandle(_vertices[k], .3f * mesh._mapGen._map._squareSize, Vector3.zero, Handles.CylinderHandleCap);

                        iterationIndex = k;

                        Vector3 oldPos = _vertices[k];
                        _vertices[k] = freeHandle;
                        deltaMove = _vertices[k] - oldPos;
                    }
                }
            }

            for (int l = 0; l < vertexIndexInEditRange.Count; l++)
            {
                if (vertexIndexInEditRange[l] != iterationIndex)
                {
                    _vertices[vertexIndexInEditRange[l]] += deltaMove;
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                mesh.EnterEditModeCreateNewMeshAndSaveData();
            }

            vertexIndexInEditRange.Clear();
            deltaMove = Vector3.zero;
        }

        void OnEnable()
        {
            mesh = (MeshGeneratorCNB)target;
        }
    }
}