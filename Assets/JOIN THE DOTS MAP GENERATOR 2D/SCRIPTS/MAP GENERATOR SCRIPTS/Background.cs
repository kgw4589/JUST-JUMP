using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CNB
{
    /// <summary>
    /// Makes the Background game object follow the "Camera.main" transform
    /// Hace que el game object "Background" siga la transform de "Camera.main".
    /// 使“Background”游戏对象跟随“Camera.main”的变换
    /// </summary>
    public class Background : MonoBehaviour
    {
        public bool Following = true;
        public Vector3 BackgroundOffset = Vector3.zero;

        protected Transform _initialParent;
        protected float _initialOffsetZ;
        protected bool _initialized = false;

        public virtual void StartFollowing()
        {
            Following = true;
            transform.SetParent(Camera.main.transform);
            if (!_initialized)
            {
                _initialOffsetZ = transform.localPosition.z;
                _initialized = true;
            }

            BackgroundOffset.z = BackgroundOffset.z + _initialOffsetZ;
            transform.localPosition = BackgroundOffset;
        }

        public virtual void StopFollowing()
        {
            Following = false;
            transform.SetParent(_initialParent);
        }

        public virtual void SetZOffset(float newOffset)
        {
            _initialOffsetZ = newOffset;
        }
    }
}