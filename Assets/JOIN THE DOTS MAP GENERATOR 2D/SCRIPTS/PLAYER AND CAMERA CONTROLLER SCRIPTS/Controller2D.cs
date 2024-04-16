using UnityEngine;
using System.Collections;

//for a deep understanding of the code check https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz

namespace CNB
{
    public class Controller2D : RaycastController
    {
        public float maxClimbAngle = 80;
        public float maxDescendAngle = 80;

        [SerializeField]
        public CollisionInfo collisions;

        [HideInInspector]
        public Vector2 playerInput;

        public override void Start()
        {
            base.Start();
            collisions.faceDir = 1;
        }

        public Vector2 Move(Vector2 moveAmount, bool standingOnPlatform)
        {
            return Move(moveAmount, Vector2.zero, standingOnPlatform);
        }

        public Vector2 Move(Vector2 moveAmount, Vector2 input, bool standingOnPlatform = false)
        {
            UpdateRaycastOrigins();

            collisions.Reset();
            collisions.moveAmountOld = moveAmount;
            playerInput = input;

            if (moveAmount.x != 0)
            {
                collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
            }

            if (moveAmount.y < 0)
            {
                DescendSlope(ref moveAmount);
            }

            HorizontalCollisions(ref moveAmount);
            if (moveAmount.y != 0)
            {
                VerticalCollisions(ref moveAmount);
            }

            transform.Translate(moveAmount);

            if (standingOnPlatform)
            {
                collisions.below = true;
            }

            return moveAmount;
        }

        void HorizontalCollisions(ref Vector2 moveAmount)
        {
            float originalMoveAmountX = moveAmount.x;
            Collider2D otherCollider = null;

            float directionX = collisions.faceDir;
            float rayLength = Mathf.Abs(moveAmount.x) + _controllerParams.skinWidth;

            if (Mathf.Abs(moveAmount.x) < _controllerParams.skinWidth)
            {
                rayLength = 2 * _controllerParams.skinWidth;
            }

            for (int i = 0; i < _controllerParams.horizontalRayCount; i++)
            {
                Vector2 rayOrigin = directionX == -1 ? _controllerParams.raycastOrigins.bottomLeft : _controllerParams.raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (_controllerParams.horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _collisionMask);

                if (hit)
                {
                    if (hit.distance == 0)
                    {
                        continue;
                    }

                    otherCollider = hit.collider;
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    collisions.slopeAngle = slopeAngle;//mio

                    if (i == 0 && slopeAngle <= maxClimbAngle)
                    {
                        if (collisions.descendingSlope)
                        {
                            collisions.descendingSlope = false;
                            moveAmount = collisions.moveAmountOld;
                        }
                        float distanceToSlopeStart = 0;
                        if (slopeAngle != collisions.slopeAngleOld)
                        {
                            distanceToSlopeStart = hit.distance - _controllerParams.skinWidth;
                            moveAmount.x -= distanceToSlopeStart * directionX;
                        }
                        ClimbSlope(ref moveAmount, slopeAngle);
                        moveAmount.x += distanceToSlopeStart * directionX;
                    }

                    if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                    {
                        moveAmount.x = (hit.distance - _controllerParams.skinWidth) * directionX;
                        rayLength = hit.distance;

                        if (collisions.climbingSlope)
                        {
                            moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                        }

                        collisions.left = directionX == -1;
                        collisions.right = directionX == 1;
                    }
                }
            }

            if (otherCollider != null && otherCollider.gameObject != gameObject && otherCollider.tag == "Pushable")
            {
                collisions.left = false;
                collisions.right = false;
            }
        }

        void VerticalCollisions(ref Vector2 moveAmount)
        {
            float directionY = Mathf.Sign(moveAmount.y);
            float rayLength = Mathf.Abs(moveAmount.y) + _controllerParams.skinWidth;

            for (int i = 0; i < _controllerParams.verticalRayCount; i++)
            {

                Vector2 rayOrigin = directionY == -1 ? _controllerParams.raycastOrigins.bottomLeft : _controllerParams.raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (_controllerParams.verticalRaySpacing * i + moveAmount.x);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, _collisionMask);

                if (hit)
                {
                    if (hit.collider.tag == "Through")
                    {
                        if (directionY == 1 || hit.distance == 0)
                        {
                            continue;
                        }
                        if (collisions.fallingThroughPlatform)
                        {
                            continue;
                        }
                        if (playerInput.y == -1)
                        {
                            collisions.fallingThroughPlatform = true;
                            Invoke("ResetFallingThroughPlatform", .5f);
                            continue;
                        }
                    }

                    moveAmount.y = (hit.distance - _controllerParams.skinWidth) * directionY;
                    rayLength = hit.distance - _controllerParams.skinWidth;

                    if (collisions.climbingSlope)
                    {
                        moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                    }

                    collisions.below = directionY == -1;
                    collisions.above = directionY == 1;
                }
            }

            if (collisions.climbingSlope)
            {
                float directionX = Mathf.Sign(moveAmount.x);
                rayLength = Mathf.Abs(moveAmount.x) + _controllerParams.skinWidth;
                Vector2 rayOrigin = (directionX == -1 ? _controllerParams.raycastOrigins.bottomLeft : _controllerParams.raycastOrigins.bottomRight) + Vector2.up * moveAmount.y;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, _collisionMask);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if (slopeAngle != collisions.slopeAngle)
                    {
                        moveAmount.x = (hit.distance - _controllerParams.skinWidth) * directionX;
                        collisions.slopeAngle = slopeAngle;
                    }
                }
            }
        }

        void ClimbSlope(ref Vector2 moveAmount, float slopeAngle)
        {
            float moveDistance = Mathf.Abs(moveAmount.x);
            float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

            if (moveAmount.y <= climbmoveAmountY)
            {
                moveAmount.y = climbmoveAmountY;
                moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                collisions.below = true;
                collisions.climbingSlope = true;
                collisions.slopeAngle = slopeAngle;
            }
        }

        void DescendSlope(ref Vector2 moveAmount)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            Vector2 rayOrigin = directionX == -1 ? _controllerParams.raycastOrigins.bottomRight : _controllerParams.raycastOrigins.bottomLeft;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Abs(moveAmount.y), _collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
                {
                    if (Mathf.Sign(hit.normal.x) == directionX)
                    {

                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= descendmoveAmountY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;

                    }
                }
            }
        }

        public struct CollisionInfo
        {
            public bool above, below;
            public bool left, right;

            public bool climbingSlope;
            public bool descendingSlope;
            public float slopeAngle, slopeAngleOld;
            public Vector2 moveAmountOld;
            public int faceDir;
            public bool fallingThroughPlatform;

            public void Reset()
            {
                above = below = false;
                left = right = false;
                climbingSlope = false;
                descendingSlope = false;

                slopeAngleOld = slopeAngle;
                slopeAngle = 0;
                faceDir = 1;
            }
        }
    }
}