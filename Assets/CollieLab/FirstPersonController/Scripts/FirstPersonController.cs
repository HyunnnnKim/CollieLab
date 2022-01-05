using CollieLab.Inputs;
using UnityEngine;

namespace CollieLab.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        #region Serialized Field
        [Header("Input Data")]
        [SerializeField] private MoveInputData moveInputData = null;
        [SerializeField] private CameraInputData cameraInputData = null;

        [Header("Camera Movement")]
        [SerializeField] private Transform cameraRoot = null;
        [SerializeField] private float rotationSpeed = 120f;

        [Header("Player Movement")]
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float runSpeed = 4f;
        [SerializeField] private float accelerate = 3f;
        [SerializeField] private float decelerate = 6f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 2f;
        #endregion

        #region Private Field
        private CharacterController controller = null;
        private float pitchAngle = 0f;
        private float horizontalVelocity = 0f;
        private float verticalVelocity = 0f;
        #endregion

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            ApplyGravity();
            JumpPlayer();
            MovePlayer();
        }

        #region Player Movement
        /// <summary>
        /// Apply gravity over time.
        /// </summary>
        private void ApplyGravity()
        {
            if (controller.isGrounded)
            {
                if (verticalVelocity < 0f)
                {
                    verticalVelocity = -2f;
                }
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// Player vertical movement.
        /// </summary>
        private void JumpPlayer()
        {
            if (moveInputData.jumpKeyDown && controller.isGrounded)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        /// <summary>
        /// Player horizontal movement.
        /// </summary>
        private void MovePlayer()
        {
            float moveInputX = moveInputData.movement.x;
            float moveInputY = moveInputData.movement.y;
            Vector3 inputDirection = transform.right * moveInputX + transform.forward * moveInputY;

            float targetSpeed = moveInputData.runKeyPressed ? runSpeed : walkSpeed;
            float currentSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
            float speedChangeRate = currentSpeed < targetSpeed ? accelerate : decelerate;

            horizontalVelocity = Mathf.Lerp(currentSpeed, inputDirection.magnitude * targetSpeed, speedChangeRate * Time.deltaTime);
            Vector3 horizontalMovement = inputDirection.normalized * horizontalVelocity * Time.deltaTime;
            Vector3 verticalMovement = new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime;

            controller.Move(horizontalMovement + verticalMovement);
        }
        #endregion

        private void LateUpdate()
        {
            RotateCamera();
        }

        #region Camera Movement
        /// <summary>
        /// Rotate player view.
        /// </summary>
        private void RotateCamera()
        {
            float yawVelocity = cameraInputData.cameraMovement.x * rotationSpeed * Time.deltaTime;
            float pitchVelocity = cameraInputData.cameraMovement.y * rotationSpeed * Time.deltaTime;

            pitchAngle -= pitchVelocity;
            pitchAngle = Mathf.Clamp(pitchAngle, -90f, 90f);

            cameraRoot.localRotation = Quaternion.Euler(pitchAngle, 0f, 0f);
            transform.Rotate(Vector3.up * yawVelocity);
        }
        #endregion

    }
}
