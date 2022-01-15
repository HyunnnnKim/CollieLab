using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CollieLab.XR.Inputs
{
    public class XRInputVisualizer : MonoBehaviour
    {
        #region Serialized Field
        [Header("Left Data")]
        [SerializeField] private ButtonHandler primaryButtonHandler_L = null;
        [SerializeField] private ButtonHandler secondaryButtonHandler_L = null;
        [SerializeField] private ButtonHandler menuButtonHandler_L = null;
        [SerializeField] private Axis2DHandler primaryAxis2DHandler_L = null;
        [SerializeField] private AxisHandler triggerAxisHandler_L = null;
        [SerializeField] private AxisHandler gripAxisHandler_L = null;

        [Header("Right Data")]
        [SerializeField] private ButtonHandler primaryButtonHandler_R = null;
        [SerializeField] private ButtonHandler secondaryButtonHandler_R = null;
        [SerializeField] private ButtonHandler menuButtonHandler_R = null;
        [SerializeField] private Axis2DHandler primaryAxis2DHandler_R = null;
        [SerializeField] private AxisHandler triggerAxisHandler_R = null;
        [SerializeField] private AxisHandler gripAxisHandler_R = null;

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

        #region Private Field
        private const string TRUE = "True";
        private const string FALSE = "False";
        #endregion

        private void OnEnable()
        {
            primaryButtonHandler_L.OnButtonDown += HandlePrimaryButtonDown_Left;
            primaryButtonHandler_L.OnButtonUp += HandlePrimaryButtonUp_Left;
            secondaryButtonHandler_L.OnButtonDown += HandleSecondaryButtonDown_Left;
            secondaryButtonHandler_L.OnButtonUp += HandleSecondaryButtonUp_Left;
            menuButtonHandler_L.OnButtonDown += HandleMenuButtonDown_Left;
            menuButtonHandler_L.OnButtonUp += HandleMenuButtonUp_Left;

            primaryButtonHandler_R.OnButtonDown += HandlePrimaryButtonDown_Right;
            primaryButtonHandler_R.OnButtonUp += HandlePrimaryButtonUp_Right;
            secondaryButtonHandler_R.OnButtonDown += HandleSecondaryButtonDown_Right;
            secondaryButtonHandler_R.OnButtonUp += HandleSecondaryButtonUp_Right;
            menuButtonHandler_R.OnButtonDown += HandleMenuButtonDown_Right;
            menuButtonHandler_R.OnButtonUp += HandleMenuButtonUp_Right;

            primaryAxis2DHandler_L.OnValueChange += HandlePrimaryAxis2D_Left;
            primaryAxis2DHandler_R.OnValueChange += HandlePrimaryAxis2D_Right;

            triggerAxisHandler_L.OnValueChange += HandleTriggerAxis_Left;
            gripAxisHandler_L.OnValueChange += HandleGripAxis_Left;

            triggerAxisHandler_R.OnValueChange += HandleTriggerAxis_Right;
            gripAxisHandler_R.OnValueChange += HandleGripAxis_Right;
        }

        #region Button
        private void HandlePrimaryButtonDown_Left(XRController controller)
        {
            if (primaryButton_L.text != TRUE)
                primaryButton_L.text = TRUE;
        }

        private void HandlePrimaryButtonUp_Left(XRController controller)
        {
            if (primaryButton_L.text != FALSE)
                primaryButton_L.text = FALSE;
        }

        private void HandleSecondaryButtonDown_Left(XRController controller)
        {
            if (secondaryButton_L.text != TRUE)
                secondaryButton_L.text = TRUE;
        }

        private void HandleSecondaryButtonUp_Left(XRController controller)
        {
            if (secondaryButton_L.text != FALSE)
                secondaryButton_L.text = FALSE;
        }

        private void HandleMenuButtonDown_Left(XRController controller)
        {
            if (menuButton_L.text != TRUE)
                menuButton_L.text = TRUE;
        }

        private void HandleMenuButtonUp_Left(XRController controller)
        {
            if (menuButton_L.text != FALSE)
                menuButton_L.text = FALSE;
        }

        private void HandlePrimaryButtonDown_Right(XRController controller)
        {
            if (primaryButton_R.text != TRUE)
                primaryButton_R.text = TRUE;
        }

        private void HandlePrimaryButtonUp_Right(XRController controller)
        {
            if (primaryButton_R.text != FALSE)
                primaryButton_R.text = FALSE;
        }

        private void HandleSecondaryButtonDown_Right(XRController controller)
        {
            if (secondaryButton_R.text != TRUE)
                secondaryButton_R.text = TRUE;
        }

        private void HandleSecondaryButtonUp_Right(XRController controller)
        {
            if (secondaryButton_R.text != FALSE)
                secondaryButton_R.text = FALSE;
        }

        private void HandleMenuButtonDown_Right(XRController controller)
        {
            if (menuButton_R.text != TRUE)
                menuButton_R.text = TRUE;
        }

        private void HandleMenuButtonUp_Right(XRController controller)
        {
            if (menuButton_R.text != FALSE)
                menuButton_R.text = FALSE;
        }
        #endregion

        #region Axis2D
        private void HandlePrimaryAxis2D_Left(XRController controller, Vector2 value)
        {
            primaryAxis2D_L.text = value.ToString();
        }

        private void HandlePrimaryAxis2D_Right(XRController controller, Vector2 value)
        {
            primaryAxis2D_R.text = value.ToString();
        }
        #endregion

        #region Axis
        private void HandleTriggerAxis_Left(XRController controller, float value)
        {
            triggerAxis_L.text = value.ToString();
        }

        private void HandleGripAxis_Left(XRController controller, float value)
        {
            gripAxis_L.text = value.ToString();
        }

        private void HandleTriggerAxis_Right(XRController controller, float value)
        {
            triggerAxis_R.text = value.ToString();
        }

        private void HandleGripAxis_Right(XRController controller, float value)
        {
            gripAxis_R.text = value.ToString();
        }
        #endregion

        private void OnDisable()
        {
            primaryButtonHandler_L.OnButtonDown -= HandlePrimaryButtonDown_Left;
            primaryButtonHandler_L.OnButtonUp -= HandlePrimaryButtonUp_Left;
            secondaryButtonHandler_L.OnButtonDown -= HandleSecondaryButtonDown_Left;
            secondaryButtonHandler_L.OnButtonUp -= HandleSecondaryButtonUp_Left;
            menuButtonHandler_L.OnButtonDown -= HandleMenuButtonDown_Left;
            menuButtonHandler_L.OnButtonUp -= HandleMenuButtonUp_Left;

            primaryButtonHandler_R.OnButtonDown -= HandlePrimaryButtonDown_Right;
            primaryButtonHandler_R.OnButtonUp -= HandlePrimaryButtonUp_Right;
            secondaryButtonHandler_R.OnButtonDown -= HandleSecondaryButtonDown_Right;
            secondaryButtonHandler_R.OnButtonUp -= HandleSecondaryButtonUp_Right;
            menuButtonHandler_R.OnButtonDown -= HandleMenuButtonDown_Right;
            menuButtonHandler_R.OnButtonUp -= HandleMenuButtonUp_Right;

            primaryAxis2DHandler_R.OnValueChange -= HandlePrimaryAxis2D_Right;
            primaryAxis2DHandler_L.OnValueChange -= HandlePrimaryAxis2D_Left;

            triggerAxisHandler_L.OnValueChange -= HandleTriggerAxis_Left;
            gripAxisHandler_L.OnValueChange -= HandleGripAxis_Left;

            triggerAxisHandler_R.OnValueChange -= HandleTriggerAxis_Right;
            gripAxisHandler_R.OnValueChange -= HandleGripAxis_Right;
        }
    }
}
