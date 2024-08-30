using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterIsoScriptable : ScriptableObject
{
    [Serializable]
    public class CharacterIso
    {
        public Sprite characterImage;
        public GameObject characterPrefab;
    }
    
    public List<CharacterIso> characterIso;
}
