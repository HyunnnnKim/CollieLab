using CollieLab.Inputs;
using UnityEngine;

namespace CollieLab.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonController : MonoBehaviour
    {
        #region Serialized Field
        [Header("Input Data")]
        [SerializeField] private MoveInputData moveInputData = null;

        [Header("Player Movement")]
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float runSpeed = 4f;
        [SerializeField] private float rotateSmoothDamp = 0.3f;
        [SerializeField] private float accelerate = 3f;
        [SerializeField] private float decelerate = 6f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float jumpHeight = 2f;
        #endregion

        #region Private Field
        private CharacterController controller = null;
        private Camera mainCamera = null;
        private float horizontalVelocity = 0f;
        private float verticalVelocity = 0f;
        private float rotateVelocity = 0f;
        Vector3 inputDirection = Vector3.zero;
        #endregion

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            mainCamera = mainCamera ?? Camera.main;
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
            if (moveInputData.HasMovement)
            {
                float moveInputX = moveInputData.movement.x;
                float moveInputY = moveInputData.movement.y;
                inputDirection = Vector3.right * moveInputX + Vector3.forward * moveInputY;
            }

            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            float rotateAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateVelocity, rotateSmoothDamp);

            if (moveInputData.HasMovement)
                transform.rotation = Quaternion.Euler(0f, rotateAngle, 0f);

            float targetSpeed = moveInputData.runKeyPressed ? runSpeed : walkSpeed;
            float currentSpeed = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
            float speedChangeRate = currentSpeed < targetSpeed ? accelerate : decelerate;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            horizontalVelocity = Mathf.Lerp(currentSpeed, inputDirection.magnitude * targetSpeed, speedChangeRate * Time.deltaTime);
            Vector3 horizontalMovement = moveDirection * horizontalVelocity * Time.deltaTime;
            Vector3 verticalMovement = new Vector3(0f, verticalVelocity, 0f) * Time.deltaTime;

            controller.Move(horizontalMovement + verticalMovement);
        }
        #endregion
    }
}
