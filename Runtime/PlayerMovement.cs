using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RADCharacterController
{
    public class PlayerMovement : MonoBehaviour
    {
        public float moveSpeed = 10f;
        public float jumpStrength = 20f;

        private bool isAirborne = true;
        private bool isDropping = false;
        private float currentVerticalMovement = 0;
        private float currentAirTime = 0;
        private Vector2 frameMovement = new Vector2();

        private GameObject currentPlatform;

        private const float EPSILON = 0.01f;
        private const float GRAVITY = 9.8f;
        private const float FLOAT_DISTANCE = 0.05f;
        private const float AIR_TIME = 0.3f;

        public void ProcessInput(float intensityHorizontal, float intensityVertical, bool didJump)
        {
            frameMovement.x = frameMovement.y = 0;

            MoveHorizontally(intensityHorizontal);
            MoveVertically(didJump, intensityVertical);

        }

        private void MoveHorizontally(float intensityHorizontal)
        {
            if (Mathf.Abs(intensityHorizontal) < EPSILON)
                return;

            frameMovement.x = intensityHorizontal * moveSpeed;
        }

        private void MoveVertically(bool didJump, float intensityVertical)
        {
            if (isAirborne)
                return;

            if (didJump)
            {
                isAirborne = true;
                currentAirTime = 0;
                currentVerticalMovement = jumpStrength;
            }
            else if (!isDropping && intensityVertical < -EPSILON && currentPlatform.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                isAirborne = true;
                currentAirTime = AIR_TIME * 0.5f;
                currentVerticalMovement = 0;
                isDropping = true;
                Invoke("SetNotDropping", 0.2f);
            }
        }

        private void SetNotDropping()
        {
            isDropping = false;
        }

        private void ProcessVerticalMovement()
        {
            if (!isAirborne)
                return;

            currentAirTime += Time.fixedDeltaTime;

            float airFloatDelta = 0.05f;

            if (currentAirTime + airFloatDelta < AIR_TIME * 0.5f)
            {
                currentVerticalMovement -= GRAVITY * Time.fixedDeltaTime;
            }
            else if (Mathf.Abs(currentAirTime - AIR_TIME * 0.5f) < airFloatDelta)
            {
                if (currentVerticalMovement > 1)
                    currentVerticalMovement -= currentVerticalMovement * Time.fixedDeltaTime * 5f;
                else
                    currentVerticalMovement -= GRAVITY * Time.fixedDeltaTime;
            }
            else
            {
                currentVerticalMovement -= GRAVITY * Time.fixedDeltaTime * 5f;
            }

            frameMovement.y = currentVerticalMovement;
        }

        private void CheckIfFloored()
        {
            if (currentVerticalMovement > 0 || isDropping)
                return;

            Debug.DrawRay(transform.position, -transform.up * (1 + FLOAT_DISTANCE), Color.cyan, 1);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, (1 + FLOAT_DISTANCE), (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Walls")));
            if (hit.collider != null && hit.distance > 0.5f)
            {
                currentPlatform = hit.collider.gameObject;
                isAirborne = false;
                transform.position = new Vector2(transform.position.x, transform.position.y + (1 + FLOAT_DISTANCE) - hit.distance);
            }
            else if (hit.collider == null && !isAirborne)
            {
                isAirborne = true;
                currentAirTime = AIR_TIME * 0.5f;
                currentVerticalMovement = -GRAVITY;
            }
        }

        private void CheckWallCollisions()
        {
            Debug.DrawRay(transform.position, transform.up * (1 + FLOAT_DISTANCE), Color.red, 1);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, (1 + FLOAT_DISTANCE), 1 << LayerMask.NameToLayer("Walls"));
            if (hit.collider != null)
            {
                isAirborne = true;
                currentVerticalMovement = Mathf.Min(currentVerticalMovement, 0);
            }

            Debug.DrawRay(transform.position, transform.right * (1 + FLOAT_DISTANCE), Color.green, 1);
            hit = Physics2D.Raycast(transform.position, transform.right, (1 + FLOAT_DISTANCE), 1 << LayerMask.NameToLayer("Walls"));
            if (hit.collider != null)
            {
                frameMovement.x = Mathf.Min(frameMovement.x, 0);
            }

            Debug.DrawRay(transform.position, -transform.right * (1 + FLOAT_DISTANCE), Color.yellow, 1);
            hit = Physics2D.Raycast(transform.position, -transform.right, (1 + FLOAT_DISTANCE), 1 << LayerMask.NameToLayer("Walls"));
            if (hit.collider != null)
            {
                frameMovement.x = Mathf.Max(frameMovement.x, 0);
            }
        }

        public void Reset()
        {
            frameMovement = Vector2.zero;
        }

        void FixedUpdate()
        {
            ProcessVerticalMovement();
            CheckWallCollisions();

            transform.Translate(frameMovement * Time.fixedDeltaTime, Space.World);

            CheckIfFloored();
        }
    }
}