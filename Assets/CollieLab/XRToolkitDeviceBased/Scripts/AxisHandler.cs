using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.XR.Interaction.Toolkit.InputHelpers;

namespace CollieLab.XR.Inputs
{
    public class AxisHandler : XRInputHandler
    {
        #region Serialized Field
        [SerializeField] private Button button = Button.None;
        #endregion

        public override void HandleInput(XRController controller)
        {
            base.HandleInput(controller);
        }
    }
}
