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
        public event Action<XRController, float> OnValueChange = null;
        #endregion

        #region Serialized Field
        [SerializeField] private Axis axis = Axis.None;
        #endregion

        #region Private Field
        private InputFeatureUsage<float> inputFeature;
        private float previousInput = 0f;
        #endregion

        public override void HandleInput(XRController controller)
        {
            float value = GetValue(controller);

            if (value != previousInput)
            {
                previousInput = value;
                OnValueChange?.Invoke(controller, value);
            }
        }

        public float GetValue(XRController controller)
        {
            if (controller.inputDevice.TryGetFeatureValue(inputFeature, out float value))
                return value;
            return 0f;
        }

        public void OnBeforeSerialize()
        {
            inputFeature = new InputFeatureUsage<float>(axis.ToString());
        }

        public void OnAfterDeserialize()
        {
            // Empty
        }

        public enum Axis { None, Trigger, Grip }
    }
}
