using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class Player : MonoBehaviour
{
    public Image image;

    private Rigidbody2D _rd;
    private LineRenderer _lineRenderer;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _direction;

    public bool isDie = false;
    private bool _isJump = false;
    public float jumpPower = 0;
    [SerializeField] private float maxPower = 5f;

    private bool _isRight = false;

    private bool _isDragging = false;


    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        image.gameObject.SetActive(false);
        _lineRenderer.enabled = false;
        _rd = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() 
            || GameManager.Instance.gameState != GameManager.GameState.Play)
        {
            _lineRenderer.enabled = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            _lineRenderer.enabled = true;
            if (EventSystem.current.IsPointerOverGameObject() 
                || GameManager.Instance.gameState != GameManager.GameState.Play)
            {
                return;
            }
            _isDragging = true;
            _startPosition = Input.mousePosition;
        }
        if (_isDragging && Input.GetMouseButton(0))
        {
            Vector2 myPos = Input.mousePosition;
            Vector2 playerLook = Camera.main.ScreenToWorldPoint(myPos);
            if (playerLook.x > transform.position.x && _isRight)
            {
                TurnPlayer();
            }
            else if (playerLook.x < transform.position.x && !_isRight)
            {
                TurnPlayer();
            }
            _endPosition = Input.mousePosition;
            _direction = _startPosition - _endPosition;
            jumpPower = _direction.magnitude / 20;
            if (jumpPower > maxPower)
            {
                jumpPower = maxPower;
            }
            _direction.Normalize();
            
            Vector3 startPos = transform.position + new Vector3(0,0.56f,0);
            Vector3 velocity = new Vector3(_direction.x, _direction.y, 0) * jumpPower;
            PredictTrajectoryAndDrawLine(startPos, velocity);
        }

        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            _lineRenderer.enabled = false;
            this.Jump(_direction);
        }
    }

    void TurnPlayer()
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
    void PredictTrajectoryAndDrawLine(Vector3 startPos, Vector3 vel)
    {
        int steps = 60;//오 진짜네
        float deltaTime = Time.fixedDeltaTime;
        Vector3 gravity = Physics.gravity;
        Vector3 position = startPos;
        Vector3 velocity = vel;

        
        _lineRenderer.positionCount = steps;// 라인 렌더러 초기화

        for (int i = 0; i < steps; i++)
        {
            position += velocity * deltaTime + 0.5f * gravity * deltaTime * deltaTime;
            velocity += gravity * deltaTime;

            _lineRenderer.SetPosition(i, position);//위치 
        }
    }

    void Jump(Vector2 dir)
    {
        _rd.AddForce(new Vector2(dir.x, dir.y) * jumpPower, ForceMode2D.Impulse);
    }
}