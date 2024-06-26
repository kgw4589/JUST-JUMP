using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaps", menuName = "Maps/New MapScriptableList")]
[System.Serializable]
public class MapListScriptable : ScriptableObject
{
    public List<MapScriptable> mapScriptableList;
}
