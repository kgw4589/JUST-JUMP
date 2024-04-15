using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CNB
{
    /// <summary>
    /// Editor class with buttons to spawn and clear spawned objects from both spawners in free cells and in map surface easily after trying different param configurations BEFORE entering EDIT MODE.
    /// Clase de editor con botones para generar y borrar objetos generados de ambos generadores en celdas libres y en la superficie del mapa fácil de usar después de probar diferentes configuraciones de parámetros ANTES de ingresar al MODO DE EDICIÓN.
    /// 编辑器类，带有按钮，用于在进入编辑模式之前尝试不同的参数设置后，在空闲单元和易于使用的地图表面上从两个生成器中生成和清除生成对象。
    /// </summary>
    [CustomEditor(typeof(MapGeneratorCNB))]
    public class MapEditorCNB : Editor
    {

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawProperties();
            serializedObject.ApplyModifiedProperties();
            SceneView.RepaintAll();
        }

        private void DrawProperties()
        {
            var map = target as MapGeneratorCNB;

            EditorGUILayout.Space();

            if (map != null&& map._mapSOLoadFrom!=null)
            {
                if (map._mapSOLoadFrom.inEditMode==false)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Spawn sprites in colliders"))
                    {
                        map.GetComponent<MeshGeneratorCNB>().SpawnInColliders(true);
                        EditorUtility.SetDirty(target);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                    }
                    if (GUILayout.Button("Spawn GO in free Cells"))
                    {
                        map.SpawnFreeCell(true);
                        EditorUtility.SetDirty(target);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                    }

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();

                    if (GUILayout.Button("Clear Collider Spawned Objects"))
                    {
                        map.GetComponent<MeshGeneratorCNB>().ClearAllColliderSpawnedObjects();
                        EditorUtility.SetDirty(target);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                    }

                    if (GUILayout.Button("Clear FreeCell Spawned Objects"))
                    {
                        map.ClearAllFreeCellSpawnedObjects();
                        EditorUtility.SetDirty(target);
                        PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Load Map From SO"))
                {
                    map.LoadMap();
                    EditorUtility.SetDirty(target);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
}