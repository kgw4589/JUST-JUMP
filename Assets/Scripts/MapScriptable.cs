using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SectionMaps
{
    public List<GameObject> sectionMaps;
}

[CreateAssetMenu(fileName = "NewMaps", menuName = "Maps/New MapMode Maps")]
[System.Serializable]
public class MapScriptable : ScriptableObject
{
    public int coinWeight = 1;
    
    public float[] section;

    public List<SectionMaps> maps;
}
