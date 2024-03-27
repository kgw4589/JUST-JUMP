using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rd;
    
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _direction;
    
    public float jumpPower = 5f;
    [SerializeField] private float maxPower = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        _rd = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startPosition = Input.mousePosition;
        }
    
        if (Input.GetMouseButtonUp(0))
        {
            _endPosition = Input.mousePosition;
            _direction = _startPosition - _endPosition;
            jumpPower = _direction.magnitude / 20; // 스와이프 길이 계산
            if (jumpPower > maxPower)
            {
                jumpPower = maxPower;
            }
            _direction.Normalize();
            Debug.Log(jumpPower);
            this.Jump(_direction);
        }
    }

    void Jump(Vector2 dir)
    {
        _rd.AddForce(new Vector2(dir.x,dir.y)* jumpPower,ForceMode2D.Impulse);
    }
}
