using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public partial class Player : MonoBehaviour
{
    private Animator _animator;
    public GameObject PausePanel;
    public Button RButton;
    public Button LButton;
    [Range(0, 0.09f)] [SerializeField] private float MoveSpeed = 0.05f;
        
    
    [SerializeField]
    private float playerHp = 5;
    [SerializeField]
    private float maxplayerHp = 5;
    private Vector3 PlayerstartPosition;

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


    private bool _isRigthButtonPush = false;
    private bool _isLeftButtonPush = false;
    
   private Image _playerHpBarColor;

   private Collider2D _collider;
   [SerializeField]
   Vector2 boxSize = new Vector2(0.8f, 0.8f); // 너비와 높이를 적절하게 설정
   [SerializeField]
   float boxAngle = 0f; // 박스의 회전 각도, 필요시 변경 가능
   
   // Start is called before the first frame update
    void Start()
    {
        //초기화
        InitAnimation();
        _collider = GetComponent<Collider2D>();
        _lineRenderer = GetComponent<LineRenderer>(); 
        _rd = GetComponent<Rigidbody2D>();
        _playerHpBarColor = PlayerHpBar.fillRect.GetComponent<Image>(); //색 변경 컴포넌트
        //------------------------------------------------
        //게임 스탯 초기화 
        PlayerstartPosition = transform.position;
        maxPower = originMaxPower; 
        playerHp = maxplayerHp;
        gravityScale = _rd.gravityScale;
        
        image.gameObject.SetActive(false);
        _lineRenderer.enabled = false;
        
        GameManager.Instance.initAction += InitObject;
    }

    void InitAnimation()
    {
        Transform firstChild = transform.GetChild(0);
        Transform secondChild = firstChild.GetChild(0);
        
        _animator = secondChild.GetComponent<Animator>();
    }

    void Update()
    {
        if (_animator == null)
        {
            InitAnimation();
        }

        if (_animator != null)
        {
            _animator.SetBool("isRun",_isLeftButtonPush || _isRigthButtonPush);
        }
        if (GameManager.Instance.gameState == GameManager.GameState.Ready)
        {
            _lineRenderer.enabled = false;
        }
        if(isDie)
        {
            _lineRenderer.enabled = false;
        }
        if (_isRigthButtonPush && !_isDragging && IsJumpAble())
        {
           
            transform.position += new Vector3(MoveSpeed, 0, 0);
            if (!_isRight)
            {
                TurnPlayer();   
            }
        }
        else if (_isLeftButtonPush && !_isDragging && IsJumpAble())
        {
           
            transform.position += new Vector3(-MoveSpeed, 0, 0);
            if (_isRight)
            {
                TurnPlayer();   
            }
        }
        if (playerHp <= 0)
        {
            isDie = true;
        }
        PlayerHpBar.value = playerHp / maxplayerHp;
        
        if (PlayerHpBar.value >= 0.8f)
        {
            _playerHpBarColor.color = Color.cyan;
        }
        else if (PlayerHpBar.value >= 0.5f)
        {
            _playerHpBarColor.color = Color.yellow;
        }
        else if (PlayerHpBar.value >= 0.2f)
        { 
            _playerHpBarColor.color = Color.red;
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
        {//클릭한게 Ui인지 확인해서 리턴함
            if (EventSystem.current.currentSelectedGameObject.name == LButton.name || 
                EventSystem.current.currentSelectedGameObject.name == RButton.name)
            {
                Debug.Log(EventSystem.current.currentSelectedGameObject.name);
                return;
            }
        }
        
        if (Input.GetMouseButtonDown(0) && IsJumpAble())
        {
            _lineRenderer.enabled = true;
            _isDragging = true;
            _startPosition = Input.mousePosition;
            // Debug.Log("시작 클릭");
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

            Vector3 startPos = _collider.bounds.center;
            Vector3 velocity = new Vector3(_direction.x, _direction.y, 0) * jumpPower / (gravityScale - (jumpPower/20));//0.6
           
            // Debug.Log("지금 클릭 중");
            PredictTrajectoryAndDrawLine(startPos, velocity);
        }

        if (_isDragging && Input.GetMouseButtonUp(0) && IsJumpAble())
        {
            GameManager.Instance.soundManager.PlaySfx(SoundManager.Sfx.jump);
            _isDragging = false;
            _animator.SetTrigger("isJump");
            StartCoroutine(Jump(_direction));
        }
    }

    public void RigthButtonPush(bool isPush)
    {
        _isRigthButtonPush = isPush;
    }
    public void LeftButtonPush(bool isPush)
    {
        _isLeftButtonPush = isPush;
    }




    bool IsJumpAble()
    {
        if ( !_isJump && !isDie && !PausePanel.activeSelf && GameManager.Instance.gameState is GameManager.GameState.Play)
        {//현재 게임상태가 펄스거나 점프안하고 있거나 안죽었거나 펄스판넬이 없을때
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
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            
            _isJump = false;
           
        }
    }

    void PredictTrajectoryAndDrawLine(Vector3 startPos, Vector3 vel)
    {
        if (_lineRenderer == null)
        {
            Debug.LogError(_lineRenderer.enabled);
            return;
        }

        int steps = 60;
        float deltaTime = Time.fixedDeltaTime;
        Vector3 gravity = Physics.gravity;
        Vector3 position = startPos;
        Vector3 velocity = vel;

        _lineRenderer.positionCount = steps;

        // 박스 캐스트에 사용할 박스의 크기와 각도를 정의
        

        for (int i = 0; i < steps; i++)
        {
            // 박스 캐스트를 사용하여 벽 충돌 감지함
            RaycastHit2D hit = Physics2D.BoxCast(position, boxSize, boxAngle, velocity, velocity.magnitude * deltaTime);
            Debug.DrawRay(position, velocity * velocity.magnitude * deltaTime, Color.red);

            if (hit.collider != null && (hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Wall")))
            {
                _lineRenderer.positionCount = i + 1;
                // 충돌이 감지되면 라인 렌더러의 길이를 충돌 지점까지로 조정함
                _lineRenderer.SetPosition(i, hit.point);

                if (hit.point.x > transform.position.x && !_isRight)
                {
                    TurnPlayer();
                }
                if (hit.point.x < transform.position.x && _isRight)
                {
                    TurnPlayer();
                }
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

    public void InitObject()
    {
        
        if (_isRight)
        {
            TurnPlayer();
        }
        _rd.velocity = Vector2.zero;
        isDie = false;
        playerHp = maxplayerHp;
        jumpPower = 0;
        maxPower = originMaxPower;
        transform.position = PlayerstartPosition;
        curruentTime = 0;
        Debug.Log("리셋");
    }
}