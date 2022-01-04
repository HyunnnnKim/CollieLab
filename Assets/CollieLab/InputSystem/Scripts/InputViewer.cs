using TMPro;
using UnityEngine;

namespace CollieLab.Inputs
{
    public class InputViewer : MonoBehaviour
    {
        #region Serialized Field
        [Header("Input Data")]
        [SerializeField] private MoveInputData moveInput = null;
        [SerializeField] private CameraInputData cameraInput = null;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI movementText = null;
        [SerializeField] private TextMeshProUGUI cameraMovementText = null;
        [SerializeField] private TextMeshProUGUI runKeyText = null;
        [SerializeField] private TextMeshProUGUI jumpKeyText = null;
        #endregion

        private void Update()
        {
            if (moveInput.HasMovement)
            {
                movementText.text = moveInput.movement.ToString();
            }
            else
            {
                string movementValue = moveInput.movement.ToString();
                if (movementText.text != movementValue)
                    movementText.text = movementValue;
            }

            if (cameraInput.HasCameraMovement)
            {
                cameraMovementText.text = cameraInput.cameraMovement.ToString();
            }
            else
            {
                string cameraMovementValue = cameraInput.cameraMovement.ToString();
                if (cameraMovementText.text != cameraMovementValue)
                    cameraMovementText.text = cameraMovementValue;
            }

            string runKeyValue = moveInput.runKeyPressed.ToString();
            if (runKeyText.text != runKeyValue)
                runKeyText.text = runKeyValue;

            string jumpKeyValue = moveInput.jumpKeyPressed.ToString();
            if (jumpKeyText.text != jumpKeyValue)
                jumpKeyText.text = jumpKeyValue;
        }
    }
}
