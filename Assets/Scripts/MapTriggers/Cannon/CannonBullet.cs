using System;
using UnityEngine;

namespace MapTriggers.Cannon
{
    public class CannonBullet : MapTriggerBasicLogic
    {
        [HideInInspector] public Rigidbody2D rb;
        [HideInInspector] public CircleCollider2D col;

        public Vector2 force;
        public float power;
    
        private Player _player;

        private float _distance;
        [HideInInspector]
        public Vector3 Pos => transform.position;

        void Awake()
        {
            _player = GameObject.FindWithTag(triggerTagName).GetComponent<Player>();
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<CircleCollider2D>();
        }
        

        public void Push(Vector2 getForce)
        {
            rb.gravityScale = 1f;
            rb.AddForce(getForce, ForceMode2D.Impulse);
        }
    

        protected override void EnterEvent()
        {
            _player.StopMove(force, power);
            
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
        }

        protected override void StayEvent()
        {
        }

        protected override void ExitEvent()
        {
        }
    }
}