using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace CollieLab.XR.Inputs
{
    public class ButtonHandler : XRInputHandler
    {
        #region Serialized Field
        [SerializeField] private event Action<XRController> OnButtonDown = null;
        [SerializeField] private event Action<XRController> OnButtonUp = null;
        #endregion

        public override void HandleInput(XRController controller)
        {
            base.HandleInput(controller);
        }
    }
}
