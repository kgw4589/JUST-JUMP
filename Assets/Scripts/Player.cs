using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public partial class Player : MonoBehaviour
{
    public Image image;
    public Image routePrediction;
    
    private Rigidbody2D _rd;
    
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private Vector2 _direction;

    [SerializeField]private bool _isJump = false;
    
    public float jumpPower = 0;
    [SerializeField] private float maxPower = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        image.gameObject.SetActive(false);
        _rd = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            image.gameObject.SetActive(true);
            _startPosition = Input.mousePosition;
            image.transform.position = transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 myPos = Input.mousePosition;
            float desiredScaleX = Vector3.Distance(myPos, _startPosition);
            if (desiredScaleX / 20 > maxPower)//애매함
            {
                desiredScaleX = maxPower * 25; // 스케일을 maxPower*40으로 제한
            }
            Debug.Log(desiredScaleX);
            image.transform.localScale = new Vector2(desiredScaleX, 1);
            image.transform.localRotation = Quaternion.Euler(0, 0, AngleInDeg(_startPosition, myPos));
        }

        if (Input.GetMouseButtonUp(0))
        {
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
            // if (!_isJump)
            // {
                this.Jump(_direction);
            //}
        }
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
        _rd.AddForce(new Vector2(dir.x,dir.y+dir.y/4)* jumpPower,ForceMode2D.Impulse);//
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
