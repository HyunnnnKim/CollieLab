using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.InputHelpers;

namespace CollieLab.XR.Inputs
{
    [CreateAssetMenu(fileName = "ButtonHandler", menuName = "CollieLab/XR/ButtonHandler")]
    public class ButtonHandler : XRInputHandler
    {
        #region Events
        public event Action<XRController> OnButtonDown = null;
        public event Action<XRController> OnButtonUp = null;
        #endregion

        #region Serialized Field
        [SerializeField] private Button button = Button.None;
        #endregion

        #region Private Field
        private bool previousInput = false;
        #endregion

        public override void HandleInput(XRController controller)
        {
            if (controller.inputDevice.IsPressed(button, out bool pressed, controller.axisToPressThreshold))
            {
                if (previousInput != pressed)
                {
                    previousInput = pressed;
                    if (pressed)
                    {
                        OnButtonDown?.Invoke(controller);
                    }
                    else
                    {
                        OnButtonUp?.Invoke(controller);
                    }
                }
            }
        }
    }
}
