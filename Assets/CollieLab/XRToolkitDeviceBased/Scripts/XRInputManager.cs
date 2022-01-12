using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CollieLab.XR.Inputs
{
    public class XRInputManager : MonoBehaviour
    {
        #region Serialized Field
        [SerializeField] private List<ButtonHandler> buttonHandlers = new List<ButtonHandler>();
        [SerializeField] private List<Axis2DHandler> axis2DHandlers = new List<Axis2DHandler>();
        [SerializeField] private List<AxisHandler> axisHandlers = new List<AxisHandler>();
        #endregion

        #region Private Field
        private XRController controller = null;
        #endregion

        private void Awake()
        {
            controller = GetComponent<XRController>();
        }

        private void Update()
        {
            HandleButtonEvents();
            HandleAxis2DEvents();
            HandleAxisEvents();
        }

        #region Handle Input
        private void HandleButtonEvents()
        {
            foreach (ButtonHandler handler in buttonHandlers)
                handler.HandleInput(controller);
        }

        private void HandleAxis2DEvents()
        {
            foreach (Axis2DHandler handler in axis2DHandlers)
                handler.HandleInput(controller);
        }

        private void HandleAxisEvents()
        {
            foreach (AxisHandler handler in axisHandlers)
                handler.HandleInput(controller);
        }
        #endregion
    }
}
