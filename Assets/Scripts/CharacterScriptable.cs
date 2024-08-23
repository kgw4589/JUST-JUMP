using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class CharacterScriptable : ScriptableObject
{
    public List<CharacterInfo> characterInfos;

    public string normalText;
    public string epicText;
    public string legendText;
    
    [System.Serializable]
    public struct CharacterInfo
    {
        public int characterNumber;
        public string characterName;
        public Sprite characterImage;
        public GameObject characterObject;

        public string ratingText;
    }
}
