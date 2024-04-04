using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class Player : MonoBehaviour
{
    public Image image;
    public Image routePrediction;

    private Rigidbody2D _rd;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _direction;

    [SerializeField] private bool _isJump = false;
    public float jumpPower = 0;
    [SerializeField] private float maxPower = 5f;

    private bool _isRight = false;
    private bool isrmrj = false;

    public bool useJump;

    private bool _isDragging = false;


    // Start is called before the first frame update
    void Start()
    {
        image.gameObject.SetActive(false);
        _rd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Debug.Log(GameManager.Instance.gameState);

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() 
                || GameManager.Instance.gameState != GameManager.GameState.Play)
            {
                Debug.Log(321312321);
                return;
            }
            
            _isDragging = true;
            
            image.gameObject.SetActive(true);
            _startPosition = Input.mousePosition;
            image.transform.position = transform.position;
        }
        if (_isDragging && Input.GetMouseButton(0))
        {
            Debug.Log(1111111);
            Vector2 myPos = Input.mousePosition;
            Vector2 playerLook = Camera.main.ScreenToWorldPoint(myPos);
            if (playerLook.x > transform.position.x && _isRight)
            {
                Right();
            }
            else if (playerLook.x < transform.position.x && !_isRight)
            {
                Right();
            }

            float desiredScaleX = Vector3.Distance(myPos, _startPosition);
            if (desiredScaleX / 28 > maxPower) //아래에 곱하는 값과 동일 한 값으로 나눠야함
            {
                desiredScaleX = maxPower * 28; //점프 최대치 5기준
            }

            Debug.Log(desiredScaleX);
            image.transform.localScale = new Vector2(desiredScaleX, 1);
            image.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(_startPosition, myPos));
        }

        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            
            image.gameObject.SetActive(false);
            _endPosition = Input.mousePosition;
            _direction = _startPosition - _endPosition;
            jumpPower = _direction.magnitude / 20;
            if (jumpPower > maxPower)
            {
                jumpPower = maxPower;
            }

            _direction.Normalize();
            Debug.Log(jumpPower);
            this.Jump(_direction);
        }
    }

    void Right()
    {
        _isRight = !_isRight;
        transform.Rotate(Vector3.up, 180f, Space.World);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.CompareTag("Ground"))
        {
            _isJump = false;
        }
    }

    void Jump(Vector2 dir)
    {
        _rd.AddForce(new Vector2(dir.x, dir.y + dir.y / 4) * jumpPower, ForceMode2D.Impulse);
    }

    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }
}