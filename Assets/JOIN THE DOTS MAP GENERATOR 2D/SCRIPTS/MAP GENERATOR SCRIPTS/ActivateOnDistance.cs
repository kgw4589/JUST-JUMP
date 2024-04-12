using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CNB
{

    /// <summary>
    /// Sets Enable and Disable distance of the game object with respect to tagged "Player"
    /// Establece "Enable y Disable distancia" del game object con respecto al game object tagged "Player"
    /// 设置游戏对象相对于标记为“Player”的游戏对象的“上下距离”
    /// </summary>
    public class ActivateOnDistance : MonoBehaviour
    {
        [Header("Thresholds")]

        /// the distance from the proximity center (the player) under which the object should be enabled
        [Tooltip("the distance from the proximity center (the player) under which the object should be enabled")]
        public float EnableDistance = 30f;
        /// the distance from the proximity center (the player) after which the object should be disabled
        [Tooltip("the distance from the proximity center (the player) after which the object should be disabled")]
        public float DisableDistance = 30f;


        [Tooltip("whether or not this object was disabled by the ProximityManager")]
        public bool DisabledByManager;


    }
}