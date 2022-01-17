using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace CollieLab.XR.Managers
{
    public class XRLocomotionManager : MonoBehaviour
    {
        #region Static Field
        private const string BaseControlScheme = "Generic XR Controller";
        private const string NoncontinuousControlScheme = "Noncontinuous Move";
        private const string ContinuousControlScheme = "Continuous Controller";
        #endregion

        #region Enums
        public enum MoveType { Noncontinuous, Continuous }
        public enum TurnType { Snap, Continuous }
        public enum ForwardSource { Head, LeftHand, RightHand }
        #endregion

        #region Serialized Field
        [Header("Locomotion Settings")]
        [Tooltip("Selected player movement type.")]
        [SerializeField] private MoveType selectedMoveType = MoveType.Noncontinuous;
        public MoveType SelectedMoveType
        {
            get => selectedMoveType;
            set
            {
                SetMoveType(value);
                selectedMoveType = value;
            }
        }

        [Tooltip("Selected player turn type.")]
        [SerializeField] private TurnType selectedTurnType = TurnType.Snap;
        public TurnType SelectedTurnType
        {
            get => selectedTurnType;
            set
            {
                SetTurnType(value);
                selectedTurnType = value;
            }
        }

        [Tooltip("Selected player forward source.")]
        [SerializeField] private ForwardSource selectedForwardSource = ForwardSource.Head;
        public ForwardSource SelectedForwardSource
        {
            get => selectedForwardSource;
            set
            {
                SetForwardSource(value);
                selectedForwardSource = value;
            }
        }

        [Header("Locomotion References")]
        [SerializeField] private List<InputActionAsset> actionAssets = new List<InputActionAsset>();
        public List<InputActionAsset> ActionAssets
        {
            get => actionAssets;
            set => actionAssets = value;
        }

        [SerializeField] private ContinuousMoveProviderBase continuousMoveProvider = null;
        public ContinuousMoveProviderBase ContinuousMoveProvider
        {
            get => continuousMoveProvider;
            set => continuousMoveProvider = value;
        }

        [SerializeField] private ContinuousTurnProviderBase continuousTurnProvider = null;
        public ContinuousTurnProviderBase ContinuousTurnProvider
        {
            get => continuousTurnProvider;
            set => continuousTurnProvider = value;
        }

        [SerializeField] private SnapTurnProviderBase snapTurnProvider = null;
        public SnapTurnProviderBase SnapTurnProvider
        {
            get => snapTurnProvider;
            set => snapTurnProvider = value;
        }

        [SerializeField] private Transform headForwardSource = null;
        public Transform HeadForwardSource
        {
            get => headForwardSource;
            set => headForwardSource = value;
        }

        [SerializeField] private Transform leftHandForwardSource = null;
        public Transform LeftHandForwardSource
        {
            get => leftHandForwardSource;
            set => leftHandForwardSource = value;
        }

        [SerializeField] private Transform rightHandForwardSource = null;
        public Transform RightHandForwardSource
        {
            get => rightHandForwardSource;
            set => rightHandForwardSource = value;
        }
        #endregion

        private void OnEnable()
        {
            SetMoveType(selectedMoveType);
            SetTurnType(selectedTurnType);
            SetForwardSource(selectedForwardSource);
        }

        private void OnDisable()
        {
            ClearBindingMasks();
        }

        #region Apply Control Scheme
        /// <summary>
        /// Setting the binding mask to selected movement scheme.
        /// </summary>
        private void SetMoveType(MoveType type)
        {
            switch (type)
            {
                case MoveType.Noncontinuous:
                    SetBindingMasks(NoncontinuousControlScheme);
                    if (continuousMoveProvider != null)
                        continuousMoveProvider.enabled = false;
                    break;

                case MoveType.Continuous:
                    SetBindingMasks(ContinuousControlScheme);
                    if (continuousMoveProvider != null)
                        continuousMoveProvider.enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Setting the turn type to Snap or Continuous.
        ///
        /// In the document, it says
        /// TODO: If the Continuous Turn and Snap Turn providers both use the same
        /// action, then disabling the first provider will cause the action to be
        /// disabled, so the action needs to be enabled, which is done by forcing
        /// the OnEnable() of the second provider to be called.
        /// </summary>
        private void SetTurnType(TurnType type)
        {
            switch (type)
            {
                case TurnType.Snap:
                    if (continuousTurnProvider != null)
                        continuousTurnProvider.enabled = false;

                    if (snapTurnProvider != null)
                    {
                        snapTurnProvider.enabled = false;
                        snapTurnProvider.enabled = true;
                        snapTurnProvider.enableTurnLeftRight = true;
                    }
                    break;

                case TurnType.Continuous:
                    if (snapTurnProvider != null)
                        snapTurnProvider.enableTurnLeftRight = false;

                    if (continuousTurnProvider != null)
                        continuousTurnProvider.enabled = true;
                    break;
            }
        }

        /// <summary>
        /// Setting the forward source of the continuous movement.
        /// </summary>
        private void SetForwardSource(ForwardSource source)
        {
            if (continuousMoveProvider == null) return;

            switch (source)
            {
                case ForwardSource.Head:
                    continuousMoveProvider.forwardSource = headForwardSource;
                    break;

                case ForwardSource.LeftHand:
                    continuousMoveProvider.forwardSource = leftHandForwardSource;
                    break;

                case ForwardSource.RightHand:
                    continuousMoveProvider.forwardSource = rightHandForwardSource;
                    break;
            }
        }

        /// <summary>
        /// Setting the binding mask to restrict action to a specific binding.
        /// </summary>
        private void SetBindingMasks(string controlSchemeName)
        {
            if (actionAssets.Count == 0) return;

            foreach (InputActionAsset asset in actionAssets)
            {
                if (asset == null) continue;

                InputControlScheme? baseInputControlScheme = FindControlScheme(BaseControlScheme, asset);
                InputControlScheme? inputControlScheme = FindControlScheme(controlSchemeName, asset);

                asset.bindingMask = GetBindingMask(baseInputControlScheme, inputControlScheme);
            }
        }

        private void ClearBindingMasks()
        {
            SetBindingMasks(string.Empty);
        }

        private InputControlScheme? FindControlScheme(string controlSchemeName, InputActionAsset asset)
        {
            if (asset == null || string.IsNullOrEmpty(controlSchemeName)) return null;

            InputControlScheme? scheme = asset.FindControlScheme(controlSchemeName);
            if (scheme == null) return null;

            return scheme;
        }

        private static InputBinding? GetBindingMask(InputControlScheme? baseInputControlScheme, InputControlScheme? inputControlScheme)
        {
            if (inputControlScheme.HasValue)
            {
                return baseInputControlScheme.HasValue
                    ? InputBinding.MaskByGroups(baseInputControlScheme.Value.bindingGroup, inputControlScheme.Value.bindingGroup)
                    : InputBinding.MaskByGroup(inputControlScheme.Value.bindingGroup);
            }

            return baseInputControlScheme.HasValue
                ? InputBinding.MaskByGroup(baseInputControlScheme.Value.bindingGroup)
                : (InputBinding?)null;
        }
        #endregion
    }
}
