using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _originPosition;
    
    public GameObject player;
    public Vector3 offset;
    
    [SerializeField] private float _targetWidth = 7.5f; // 가로 방향으로 보여주고 싶은 유닛의 수

    private Camera _camera;

    void Start()
    {
        _originPosition = transform.position;
        _camera = GetComponent<Camera>();
        GameManager.Instance.startAction += InitCamera;
        InitCamera();
    }

    public void InitCamera()
    {
        transform.position = _originPosition;
        Debug.Log(_originPosition);
        // 가로세로 비율에 따라 orthographicSize 조정
        float windowAspect = (float)Screen.width / (float)Screen.height;
        _camera.orthographicSize = (_targetWidth / windowAspect) / 2f;
    }

    void Update()
    {
        var dirPosition = player.transform.position + offset;
        Vector3 moveDir = new Vector3(transform.position.x, dirPosition.y, transform.position.z);
        
        transform.position = Vector3.Lerp(transform.position, moveDir, Time.deltaTime);
    }
}