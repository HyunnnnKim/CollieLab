using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CollieLab.XR.Inputs
{
    public class XRInputVisualizer : MonoBehaviour
    {
        #region Serialized Field
        [Header("Left Data")]
        [SerializeField] private List<ButtonHandler> buttonHandlers_L = new List<ButtonHandler>();
        [SerializeField] private List<Axis2DHandler> axis2DHandlers_L = new List<Axis2DHandler>();
        [SerializeField] private List<AxisHandler> axisHandlers_L = new List<AxisHandler>();

        [Header("Right Data")]
        [SerializeField] private List<ButtonHandler> buttonHandlers_R = new List<ButtonHandler>();
        [SerializeField] private List<Axis2DHandler> axis2DHandlers_R = new List<Axis2DHandler>();
        [SerializeField] private List<AxisHandler> axisHandlers_R = new List<AxisHandler>();

        [Header("Left UI")]
        [SerializeField] private TextMeshProUGUI secondaryAxis2D = null;
        [SerializeField] private TextMeshProUGUI triggerAxis_L = null;
        [SerializeField] private TextMeshProUGUI gripAxis_L = null;
        [SerializeField] private TextMeshProUGUI primaryButton_L = null;
        [SerializeField] private TextMeshProUGUI secondaryButton_L = null;
        [SerializeField] private TextMeshProUGUI menuButton_L = null;

        [Header("Right UI")]
        [SerializeField] private TextMeshProUGUI primaryAxis2D = null;
        [SerializeField] private TextMeshProUGUI triggerAxis_R = null;
        [SerializeField] private TextMeshProUGUI gripAxis_R = null;
        [SerializeField] private TextMeshProUGUI primaryButton_R = null;
        [SerializeField] private TextMeshProUGUI secondaryButton_R = null;
        [SerializeField] private TextMeshProUGUI menuButton_R = null;
        #endregion

        private void OnEnable()
        {
            foreach (ButtonHandler handler in buttonHandlers_L)
            {
                handler.OnButtonDown += HandleButtonDownUI;
                handler.OnButtonUp += HandleButtonUpUI;
            }
        }

        #region UI
        /// <summary>
        /// 
        /// </summary>
        private void HandleButtonDownUI(XRController controller)
        {
            string name = controller.inputDevice.name;
            Debug.Log("InputDeviceName: " + name);
            string activateUsage = controller.activateUsage.ToString();
            Debug.Log("ActivateUsage: " + activateUsage);
            var selectUsage = controller.selectUsage.ToString();
            Debug.Log("SelectUsage: " + selectUsage);
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleButtonUpUI(XRController controller)
        {
            
        }
        #endregion

        private void OnDisable()
        {
            foreach (ButtonHandler handler in buttonHandlers_L)
            {
                handler.OnButtonDown -= HandleButtonDownUI;
                handler.OnButtonUp -= HandleButtonUpUI;
            }
        }
    }
}
