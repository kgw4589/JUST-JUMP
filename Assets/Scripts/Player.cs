using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rd;
    
    private Vector2 strPos;
    private Vector2 endPos;
    private Vector2 direction;
    
    public float JumpPower = 5f;
    [SerializeField] private float maxPower = 5f;

    
    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            strPos = Input.mousePosition;
        }
    
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            direction = strPos - endPos;
            JumpPower = direction.magnitude/20; // 스와이프 길이 계산
            if (JumpPower > maxPower)
            {
                JumpPower = maxPower;
            }
            direction.Normalize();
            Debug.Log(JumpPower);
            Jump(direction);
        }
    }

    void Jump(Vector2 dir)
    {
        rd.AddForce(new Vector2(dir.x,dir.y)* JumpPower,ForceMode2D.Impulse);
    }
}
