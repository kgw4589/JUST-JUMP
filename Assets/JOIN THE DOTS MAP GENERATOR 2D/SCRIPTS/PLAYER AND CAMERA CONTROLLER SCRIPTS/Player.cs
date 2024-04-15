using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using CNB;

//for a deep understanding of the code check https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz


namespace CNB
{
	[RequireComponent(typeof(Controller2D))]
	public class Player : MonoBehaviour
	{
		public GameObject _renderer;
		public Controller2D _controller;
		public int newX;
		public int NumberOfJumps = 2;
		public int NumberOfJumpsLeft;
		private InputAsset _playerInputActions;

		public float maxJumpHeight = 8;
		public float minJumpHeight = 1;
		public float timeToJumpApex = .4f;
		float accelerationTimeAirborne = .2f;
		float accelerationTimeGrounded = .1f;
		public float moveSpeed;
		public float walkSpeed = 10;

		public float gravity;
		float maxJumpVelocity;
		float minJumpVelocity;
		public Vector3 velocity;
		float velocityXSmoothing;

		public Vector2 directionalInput;

		protected void Start()
		{
			_controller = GetComponent<Controller2D>();
			_playerInputActions = new InputAsset();
			_playerInputActions.PlayerControls.Enable();

			gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
			maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
			minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		}
		void FixedUpdate()
		{
			CalculateVelocity();
			_controller.Move(velocity * Time.deltaTime, directionalInput);
			if (_controller.collisions.above || _controller.collisions.below)
			{
				velocity.y = 0;
			}
		}

		public void SetPos()
        {
			Vector3 posToSpawnPlayer = GameObject.FindGameObjectWithTag("SpawnPlayerPos") ? GameObject.FindGameObjectWithTag("SpawnPlayerPos").transform.localPosition : Vector3.zero;
			transform.position = posToSpawnPlayer;
		}

		public void SetDirectionalInput(InputAction.CallbackContext context)
		{
			directionalInput = context.ReadValue<Vector2>();
		}

		public void SetDirectionalInput(Vector2 v)
		{
			directionalInput = v;
		}

		public void Jump(InputAction.CallbackContext context)
		{
			if (context.action.WasPressedThisFrame())
			{
				OnJumpInputDown();
			}
			else if (context.action.WasReleasedThisFrame())
			{
				OnJumpInputUp();
			}
		}

		public void OnJumpInputDown()
		{

			if (_controller.collisions.below)
			{
				velocity.y = maxJumpVelocity;
				ResetNumberOfJumps();
				NumberOfJumpsLeft--;
				return;
			}
			else
			{
				if (NumberOfJumpsLeft > 0)
				{
					velocity.y = maxJumpVelocity;
					NumberOfJumpsLeft--;
				}
			}
		}

		public void OnJumpInputUp()
		{
			if (velocity.y > minJumpVelocity)
			{
				velocity.y = minJumpVelocity;
			}
		}

		public virtual void ResetNumberOfJumps()
		{
			NumberOfJumpsLeft = NumberOfJumps;
		}

		void CalculateVelocity()
		{
			float targetVelocityX = directionalInput.x * moveSpeed;
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (_controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.y += gravity * Time.deltaTime;
		}
	}
}
