using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    private float _targetWidth = 6f; // 가로 방향으로 보여주고 싶은 유닛의 수

    void Start()
    {
        var camera = GetComponent<Camera>();

        // 가로세로 비율에 따라 orthographicSize 조정
        float windowAspect = (float)Screen.width / (float)Screen.height;
        camera.orthographicSize = (_targetWidth / windowAspect) / 2f;
    }

    void Update()
    {
        var dirPos = player.transform.position + offset;
        
        // 카메라를 플레이어를 따라가게 하되, z축은 변경하지 않음
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, dirPos.y, transform.position.z), Time.deltaTime);
    }
}