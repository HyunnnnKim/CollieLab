using UnityEngine;

namespace CollieLab.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        #region Serialized Field
        [Header("Input Data")]
        [SerializeField] private MoveInputData moveInputData = null;
        [SerializeField] private CameraInputData cameraInputData = null;

        [Header("Input Key")]
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        #endregion

        private void Update()
        {
            GetMovementInput();
            GetCameraMovementInput();
        }

        #region Input Data
        /// <summary>
        /// Get movement related input data.
        /// </summary>
        private void GetMovementInput()
        {
            moveInputData.movement.x = Input.GetAxis("Horizontal");
            moveInputData.movement.y = Input.GetAxis("Vertical");

            moveInputData.runKeyDown = Input.GetKeyDown(runKey);
            moveInputData.runKeyUp = Input.GetKeyUp(runKey);
            moveInputData.runKeyPressed = Input.GetKey(runKey);

            moveInputData.jumpKeyDown = Input.GetKeyDown(jumpKey);
            moveInputData.jumpKeyUp = Input.GetKeyUp(jumpKey);
            moveInputData.jumpKeyPressed = Input.GetKey(jumpKey);
        }

        /// <summary>
        /// Get camera movement related input data.
        /// </summary>
        private void GetCameraMovementInput()
        {
            cameraInputData.cameraMovement.x = Input.GetAxis("Mouse X");
            cameraInputData.cameraMovement.y = Input.GetAxis("Mouse Y");
        }
        #endregion
    }
}
