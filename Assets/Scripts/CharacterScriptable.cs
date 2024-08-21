using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class CharacterScriptable : ScriptableObject
{
    public List<CharacterInfo> normalCharacters;
    public List<CharacterInfo> epicCharacters;
    public List<CharacterInfo> legendCharacters;

    public string normalText;
    public string epicText;
    public string legendText;
    
    [System.Serializable]
    public struct CharacterInfo
    {
        public string characterName;
        public Sprite characterImage;
        public GameObject characterObject;

        public string ratingText;
    }
}
