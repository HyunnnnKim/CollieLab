using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CollieLab.XR.Inputs
{
    public class XRActionBasedInputViewer : MonoBehaviour
    {
        #region Serialized Field
        [Header("Left Input")]
        [SerializeField] private InputActionReference primaryButtonRef_Left = null;
        [SerializeField] private InputActionReference secondaryButtonRef_Left = null;
        [SerializeField] private InputActionReference menuButtonRef_Left = null;
        [SerializeField] private InputActionReference primaryAxis2DRef_Left = null;
        [SerializeField] private InputActionReference triggerAxisRef_Left = null;
        [SerializeField] private InputActionReference gripAxisRef_Left = null;

        [Header("Right Input")]
        [SerializeField] private InputActionReference primaryButtonRef_Right = null;
        [SerializeField] private InputActionReference secondaryButtonRef_Right = null;
        [SerializeField] private InputActionReference menuButtonRef_Right = null;
        [SerializeField] private InputActionReference primaryAxis2DRef_Right = null;
        [SerializeField] private InputActionReference triggerAxisRef_Right = null;
        [SerializeField] private InputActionReference gripAxisRef_Right = null;

        [Header("Left UI")]
        [SerializeField] private TextMeshProUGUI primaryButton_L = null;
        [SerializeField] private TextMeshProUGUI secondaryButton_L = null;
        [SerializeField] private TextMeshProUGUI menuButton_L = null;
        [SerializeField] private TextMeshProUGUI primaryAxis2D_L = null;
        [SerializeField] private TextMeshProUGUI triggerAxis_L = null;
        [SerializeField] private TextMeshProUGUI gripAxis_L = null;

        [Header("Right UI")]
        [SerializeField] private TextMeshProUGUI primaryButton_R = null;
        [SerializeField] private TextMeshProUGUI secondaryButton_R = null;
        [SerializeField] private TextMeshProUGUI menuButton_R = null;
        [SerializeField] private TextMeshProUGUI primaryAxis2D_R = null;
        [SerializeField] private TextMeshProUGUI triggerAxis_R = null;
        [SerializeField] private TextMeshProUGUI gripAxis_R = null;
        #endregion

        private void OnEnable()
        {
            primaryButtonRef_Left.action.performed += HandlePrimaryButton_Left;
            secondaryButtonRef_Left.action.performed += HandleSecondaryButton_Left;
            menuButtonRef_Left.action.performed += HandleMenuButton_Left;
            primaryButtonRef_Right.action.performed += HandlePrimaryButton_Right;
            secondaryButtonRef_Right.action.performed += HandleSecondaryButton_Right;
            menuButtonRef_Right.action.performed += HandleMenuButton_Right;

            primaryAxis2DRef_Left.action.performed += HandlePrimaryAxis2D_Left;
            primaryAxis2DRef_Right.action.performed += HandlePrimaryAxis2D_Right;

            triggerAxisRef_Left.action.performed += HandleTriggerAxis_Left;
            gripAxisRef_Left.action.performed += HandleGripAxis_Left;
            triggerAxisRef_Right.action.performed += HandleTriggerAxis_Right;
            gripAxisRef_Right.action.performed += HandleGripAxis_Right;
        }

        #region Button
        private void HandlePrimaryButton_Left(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();
            primaryButton_L.text = input.ToString();
        }

        private void HandleSecondaryButton_Left(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();
            secondaryButton_L.text = input.ToString();
        }

        private void HandleMenuButton_Left(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();
            menuButton_L.text = input.ToString();
        }

        private void HandlePrimaryButton_Right(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();
            primaryButton_R.text = input.ToString();
        }

        private void HandleSecondaryButton_Right(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();
            secondaryButton_R.text = input.ToString();
        }

        private void HandleMenuButton_Right(InputAction.CallbackContext context)
        {
            bool input = context.ReadValueAsButton();
            menuButton_R.text = input.ToString();
        }
        #endregion

        #region Axis2D
        private void HandlePrimaryAxis2D_Left(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            primaryAxis2D_L.text = input.ToString();
        }

        private void HandlePrimaryAxis2D_Right(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            primaryAxis2D_R.text = input.ToString();
        }
        #endregion

        #region Axis
        private void HandleTriggerAxis_Left(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();
            triggerAxis_L.text = input.ToString();
        }

        private void HandleGripAxis_Left(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();
            gripAxis_L.text = input.ToString();
        }

        private void HandleTriggerAxis_Right(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();
            triggerAxis_R.text = input.ToString();
        }

        private void HandleGripAxis_Right(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();
            gripAxis_R.text = input.ToString();
        }
        #endregion

        private void OnDisable()
        {
            primaryButtonRef_Left.action.performed -= HandlePrimaryButton_Left;
            secondaryButtonRef_Left.action.performed -= HandleSecondaryButton_Left;
            menuButtonRef_Left.action.performed -= HandleMenuButton_Left;
            primaryButtonRef_Right.action.performed -= HandlePrimaryButton_Right;
            secondaryButtonRef_Right.action.performed -= HandleSecondaryButton_Right;
            menuButtonRef_Right.action.performed -= HandleMenuButton_Right;

            primaryAxis2DRef_Left.action.performed -= HandlePrimaryAxis2D_Left;
            primaryAxis2DRef_Right.action.performed -= HandlePrimaryAxis2D_Right;

            triggerAxisRef_Left.action.performed -= HandleTriggerAxis_Left;
            gripAxisRef_Left.action.performed -= HandleGripAxis_Left;
            triggerAxisRef_Right.action.performed -= HandleTriggerAxis_Right;
            gripAxisRef_Right.action.performed -= HandleGripAxis_Right;
        }
    }
}
