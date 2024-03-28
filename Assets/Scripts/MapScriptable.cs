using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMaps", menuName = "Maps/New MapMode Maps")]
[System.Serializable]
public class MapScriptable : ScriptableObject
{
    public List<GameObject> maps;
}
