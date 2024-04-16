using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.Events;

namespace CNB
{
    /// <summary>
    /// Simple helper class to interact with "Cinemachine" so everithing works out of the box.
    /// Clase de ayuda simple para interactuar con "Cinemachine" para que todo funcione de inmediato.
    /// 与“Cinemachine”交互的简单帮助程序类，让一切开箱即用。
    /// </summary>
    public class CameraCNB : MonoBehaviour
    {
        //UI
        public Vector2 _followSpeed = new Vector2(0f, 10f);
        public Vector2 _zoom = new Vector2(6f, 10f);
        public float _initialZoom = 6f;
        public float _zoomSpeed = 0.3f;
        //
        Player _targetCharacter;
        GameObject _map;
        BoxCollider _boundsCol;
        CinemachineFramingTransposer _framingTransposer;
        CinemachineVirtualCamera _virtualCamera;
        CinemachineConfiner _confiner;
        float _currentZoom;
        bool _initialized = false;
        bool _ConfInitialized = false;
        Background _targetLevelBackground;

        protected virtual void Awake()
        {
            Initialization();
        }

        protected virtual void Initialization()
        {
            if (_initialized)
            {
                return;
            }
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _confiner = GetComponent<CinemachineConfiner>();
            _confiner.m_ConfineMode = CinemachineConfiner.Mode.Confine3D;
            _currentZoom = _initialZoom;
            _framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _targetLevelBackground = GameObject.FindGameObjectWithTag("BackGround").GetComponent<Background>();
            _targetCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            _map = GameObject.FindGameObjectWithTag("Map");
            _boundsCol = GameObject.FindGameObjectWithTag("MapBounds").GetComponent<BoxCollider>();
            _targetLevelBackground?.StartFollowing();
            _initialized = true;
        }

        protected virtual void Start()
        {
            InitializeConfiner();
            _virtualCamera.m_Lens.OrthographicSize = _initialZoom;
            StartFollowing();
        }

        protected virtual void InitializeConfiner()
        {
            if (_confiner != null)
            {
                if (_confiner.m_ConfineMode == CinemachineConfiner.Mode.Confine3D)
                {
                    _confiner.m_BoundingVolume = _boundsCol;
                    _ConfInitialized = _boundsCol != null ? true : false;
                }
            }
        }

        public virtual void StartFollowing()
        {
            Initialization();
            _virtualCamera.Follow = _targetCharacter.gameObject.transform;
            _virtualCamera.enabled = true;
        }

        public virtual void StopFollowing()
        {
            Initialization();
            _virtualCamera.Follow = null;
            _virtualCamera.enabled = false;
        }

        protected virtual void LateUpdate()
        {
            PerformOrthographicZoom();
            if (_ConfInitialized == false)
            {
                InitializeConfiner();
            }
        }
        
        protected virtual void PerformOrthographicZoom()
        {
            float characterSpeed = Mathf.Abs(_targetCharacter.velocity.x);
            float currentVelocity = Mathf.Max(characterSpeed, _followSpeed.x);
            float targetZoom = Remap(currentVelocity, _followSpeed.x, _followSpeed.y, _zoom.x, _zoom.y);
            _currentZoom = Mathf.Lerp(_currentZoom, targetZoom, Time.deltaTime * _zoomSpeed);
            _virtualCamera.m_Lens.OrthographicSize = _currentZoom;
        }
        
        public float Remap(float x, float A, float B, float C, float D)
        {
            float remappedValue = C + (x - A) / (B - A) * (D - C);
            return remappedValue;
        }

    }
}