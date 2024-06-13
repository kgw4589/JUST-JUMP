using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SectionMaps
{
    public List<GameObject> sectionMaps;
}

[CreateAssetMenu(fileName = "NewMaps", menuName = "Maps/New MapMode Maps")]
[System.Serializable]
public class MapScriptable : ScriptableObject
{
    public float[] section;

    public List<SectionMaps> maps;
}
