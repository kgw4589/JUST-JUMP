using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class Player : MonoBehaviour
{
    public GameObject PausePanel;
    public Button RigthButton;
    public Button LeftButton;
    
    [SerializeField]
    private float playerHp = 5;
    [SerializeField]
    private float maxplayerHp = 5;

    [SerializeField] 
    private float moveSpeed = 3f;

    public Slider PlayerHpBar;
    public bool isHitWave = false;
    public Image image;
    public float curruentTime = 0f;
    private float _healTime = 1f;

    private Rigidbody2D _rd;
    private LineRenderer _lineRenderer;

    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _direction;
    [SerializeField]
    private float gravityScale;
    public bool isDie = false;
    [SerializeField]
    private bool _isJump = false;
    public float jumpPower = 0;
    [SerializeField] 
    private float originMaxPower = 8f;
    [SerializeField] 
    private float maxPower;

    private bool _isRight = false;

    private bool _isDragging = false;
    
    
    
   private Color _playerHpBarColor;
   

    // Start is called before the first frame update
    void Start()
    {
        maxPower = originMaxPower; 
        playerHp = maxplayerHp;
        _lineRenderer = GetComponent<LineRenderer>(); 
        image.gameObject.SetActive(false);
        _lineRenderer.enabled = false;
        _rd = GetComponent<Rigidbody2D>();
        gravityScale = _rd.gravityScale;
        _playerHpBarColor = PlayerHpBar.fillRect.GetComponent<Image>().color; //색 변경 컴포넌트
        RigthButton.onClick.AddListener(RButton);
        LeftButton.onClick.AddListener(LButton);
    }

    void RButton()
    {
        if (!_isDragging && IsJumpAble())
        {
            transform.position += new Vector3(-0.2f, 0, 0);   
            if (_isRight)
            {
                TurnPlayer();   
            }
        }
       
    }
    void LButton()
    {
        if (!_isDragging && IsJumpAble())
        {
            transform.position += new Vector3(0.2f, 0, 0);
            if (!_isRight)
            {
                TurnPlayer();   
            }
        }
    }
    
    void Update()
    {
        if (playerHp <= 0)
        {
            isDie = true;
        }
        PlayerHpBar.value = playerHp / maxplayerHp;
        
        if (PlayerHpBar.value >= 0.8f)
        {
            PlayerHpBar.fillRect.GetComponent<Image>().color = Color.cyan;
        }
        else if (PlayerHpBar.value >= 0.5f)
        {
            PlayerHpBar.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else if (PlayerHpBar.value >= 0.2f)
        {
            
            PlayerHpBar.fillRect.GetComponent<Image>().color = Color.red;
            //_playerHpBarColor = Color.red;
        }
        
        if (!isHitWave && playerHp <= maxplayerHp && !isDie) //힐량 오버남
        {
            curruentTime += Time.deltaTime;
            if (curruentTime >= _healTime)
            {
                playerHp += Time.deltaTime;
            }
        }
    
        if (Input.GetMouseButtonDown(0) && (EventSystem.current.currentSelectedGameObject))
        {
            if (EventSystem.current.currentSelectedGameObject.name == LeftButton.name
                || EventSystem.current.currentSelectedGameObject.name == RigthButton.name)
            {
                Debug.Log(EventSystem.current.currentSelectedGameObject.name);
                return;
            }
        }

        if (EventSystem.current.IsPointerOverGameObject()
            || GameManager.Instance.gameState != GameManager.GameState.Play)
        {
            _lineRenderer.enabled = false;
        }
        if (Input.GetMouseButtonDown(0) && IsJumpAble())
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

        if (Input.GetMouseButton(0) && IsJumpAble() && _isDragging )
        {
            //Vector2 myPos = Input.mousePosition;
            //Vector2 playerLook = Camera.main.ScreenToWorldPoint(myPos);

            _endPosition = Input.mousePosition;
            _direction = _startPosition - _endPosition;
            jumpPower = _direction.magnitude / 20;
            if (jumpPower > maxPower)
            {
                jumpPower = maxPower;
            }

            _direction.Normalize();

            Vector3 startPos = transform.position;
            Vector3 velocity = new Vector3(_direction.x, _direction.y, 0) * jumpPower / (gravityScale - (jumpPower/20));//0.6
            _lineRenderer.transform.position = transform.position;
            PredictTrajectoryAndDrawLine(startPos, velocity);
        }

        if (_isDragging && Input.GetMouseButtonUp(0) && IsJumpAble())
        {
            SoundManager.Instance.PlaySfx(SoundManager.Sfx.jump);
            _isDragging = false;
            StartCoroutine(Jump(_direction));
        }
    }

    bool IsJumpAble()
    {
        if ( GameManager.Instance.gameState is not GameManager.GameState.Pause && !_isJump && !isDie && !PausePanel.activeSelf)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void TurnPlayer()
    {
        _isRight = !_isRight;
        transform.Rotate(Vector3.up, 180f, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        maxPower = originMaxPower;
        _lineRenderer.transform.position = transform.position;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _lineRenderer.transform.position = transform.position;
            _isJump = false;
           
        }
    }

    void PredictTrajectoryAndDrawLine(Vector3 startPos, Vector3 vel)
    {
        int steps = 60;
        float deltaTime = Time.fixedDeltaTime;
        Vector3 gravity = Physics.gravity;
        Vector3 position = startPos;
        Vector3 velocity = vel;

        _lineRenderer.positionCount = steps;

        for (int i = 0; i < steps; i++)
        {
            // 레이캐스트를 사용하여 벽 충돌 감지
            RaycastHit2D hit = Physics2D.Raycast(position, velocity, velocity.magnitude * deltaTime);
            if (hit.collider != null && (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall")))
            {
                if (hit.transform.position.x < transform.position.x && _isRight)
                {
                    TurnPlayer();
                }
                if (hit.transform.position.x > transform.position.x && !_isRight)
                {
                    TurnPlayer();
                }
                // 충돌이 감지되면 라인 렌더러의 길이를 충돌 지점까지로 조정
                _lineRenderer.positionCount = i + 1;
                _lineRenderer.SetPosition(i, hit.point);
                break; // 충돌이 감지되면 루프를 빠져나옴
            }
            else
            {
                // 충돌이 감지되지 않으면 정상적으로 라인 렌더러 그리기 계속
                position += velocity * deltaTime + gravity * (0.5f * deltaTime * deltaTime);
                velocity += gravity * deltaTime;
                _lineRenderer.SetPosition(i, position);
                
            }
        }
    }

    public void Damage()
    {
        if (!isDie)
        {
            curruentTime = 0;
            playerHp -= Time.deltaTime;
            isHitWave = true;
        }
    }

    IEnumerator Jump(Vector2 dir)
    {
        _rd.AddForce(new Vector2(dir.x, dir.y) * jumpPower, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.09f);
        
        _isJump = true;
        _lineRenderer.transform.position = transform.position;
        _lineRenderer.enabled = false;
    }
}