using System;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace CollieLab.XR.Inputs
{
    [CreateAssetMenu(fileName = "AxisHandler", menuName = "CollieLab/XR/AxisHandler")]
    public class AxisHandler : XRInputHandler, ISerializationCallbackReceiver
    {
        #region Events
        public event Action<XRController, Vector2> OnValueChange = null;
        #endregion

        #region Serialized Field
        [SerializeField] private Axis2D axis = Axis2D.None;
        #endregion

        #region Private Field
        private InputFeatureUsage<Vector2> inputFeature;
        private Vector2 previousInput = Vector2.zero;
        #endregion

        public override void HandleInput(XRController controller)
        {
            Vector2 value = GetValue(controller);

            if (value != previousInput)
            {
                previousInput = value;
                OnValueChange?.Invoke(controller, value);
            }
        }

        public Vector2 GetValue(XRController controller)
        {
            if (controller.inputDevice.TryGetFeatureValue(inputFeature, out Vector2 value))
                return value;
            return Vector2.zero;
        }

        public void OnBeforeSerialize()
        {
            // Empty
        }

        public void OnAfterDeserialize()
        {
            inputFeature = new InputFeatureUsage<Vector2>(axis.ToString());
        }

        public enum Axis2D { None, Primary2DAxis, Secondary2DAxis }
    }
}
