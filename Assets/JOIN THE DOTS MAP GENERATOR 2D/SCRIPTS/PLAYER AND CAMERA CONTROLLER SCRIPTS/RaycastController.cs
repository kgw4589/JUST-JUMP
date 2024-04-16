using UnityEngine;
using System.Collections;


//for a deep understanding of the code check https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz

namespace CNB
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class RaycastController : MonoBehaviour
	{
		public LayerMask _collisionMask;

		public ControllerParameters _controllerParams;

		public virtual void Awake()
		{
			_controllerParams.collider = GetComponent<BoxCollider2D>();
		}

		public virtual void Start()
		{
			CalculateRaySpacing();
		}

		public void UpdateRaycastOrigins()
		{
			Bounds bounds = _controllerParams.collider.bounds;
			bounds.Expand(_controllerParams.skinWidth * -2);

			_controllerParams.raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
			_controllerParams.raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
			_controllerParams.raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
			_controllerParams.raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
		}

		public void CalculateRaySpacing()
		{
			Bounds bounds = GetComponent<BoxCollider2D>().bounds;
			bounds.Expand(_controllerParams.skinWidth * -2);

			float boundsWidth = bounds.size.x;
			float boundsHeight = bounds.size.y;

			_controllerParams.horizontalRayCount = Mathf.RoundToInt(boundsHeight / _controllerParams.dstBetweenRays);
			_controllerParams.verticalRayCount = Mathf.RoundToInt(boundsWidth / _controllerParams.dstBetweenRays);

			_controllerParams.horizontalRaySpacing = bounds.size.y / (_controllerParams.horizontalRayCount - 1);
			_controllerParams.verticalRaySpacing = bounds.size.x / (_controllerParams.verticalRayCount - 1);
		}

		public struct RaycastOrigins
		{
			public Vector2 topLeft, topRight;
			public Vector2 bottomLeft, bottomRight;
		}

		[System.Serializable]
		public class ControllerParameters
		{
			[HideInInspector]
			public float skinWidth = .015f;
			public float dstBetweenRays = .1f;
			[HideInInspector]
			public int horizontalRayCount;
			[HideInInspector]
			public int verticalRayCount;
			[HideInInspector]
			public float horizontalRaySpacing;
			[HideInInspector]
			public float verticalRaySpacing;

			[HideInInspector]
			public BoxCollider2D collider;
			public RaycastOrigins raycastOrigins;
		}
	}
}
