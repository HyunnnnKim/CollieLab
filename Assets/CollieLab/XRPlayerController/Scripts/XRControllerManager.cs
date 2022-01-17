using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace CollieLab.XR.Managers
{
    [DefaultExecutionOrder(ControllerManagerUpdateOrder)]
    public class XRControllerManager : MonoBehaviour
    {
        #region Static Field
        public const int ControllerManagerUpdateOrder = 10;
        #endregion

        #region Serialized Field
        [Header("Controller GameObjects")]
        [FormerlySerializedAs("BaseControllerGO")]
        [SerializeField] private GameObject baseControllerGameObject = null;
        public GameObject BaseControllerGameObject
        {
            get => baseControllerGameObject;
            set => baseControllerGameObject = value;
        }

        [FormerlySerializedAs("TeleportControllerGO")]
        [SerializeField] private GameObject teleportControllerGameObject = null;
        public GameObject TeleportControllerGameObject
        {
            get => teleportControllerGameObject;
            set => teleportControllerGameObject = value;
        }

        [Header("References")]
        [SerializeField] private InputActionReference teleportModeActivate = null;
        public InputActionReference TeleportModeActivate
        {
            get => teleportModeActivate;
            set => teleportModeActivate = value;
        }

        [SerializeField] private InputActionReference teleportModeCancel = null;
        public InputActionReference TeleportModeCancel
        {
            get => teleportModeCancel;
            set => teleportModeCancel = value;
        }

        [SerializeField] private InputActionReference turn = null;
        public InputActionReference Turn
        {
            get => turn;
            set => turn = value;
        }

        [SerializeField] private InputActionReference move = null;
        public InputActionReference Move
        {
            get => move;
            set => move = value;
        }

        [FormerlySerializedAs("TranslateObject")]
        [SerializeField] private InputActionReference translateAnchor = null;
        public InputActionReference TranslateAnchor
        {
            get => translateAnchor;
            set => translateAnchor = value;
        }

        [FormerlySerializedAs("RotateObject")]
        [SerializeField] private InputActionReference rotateAnchor = null;
        public InputActionReference RotateAnchor
        {
            get => rotateAnchor;
            set => rotateAnchor = value;
        }

        [Header("Default States")]
#pragma warning disable IDE0044 // Add readonly modifier -- readonly fields cannot be serialized by Unity
        [SerializeField] private ControllerState selectState = new ControllerState(StateID.Select);
        public ControllerState SelectedState => selectState;

        [SerializeField] private ControllerState teleportState = new ControllerState(StateID.Teleport);
        public ControllerState TeleportState => teleportState;

        [SerializeField] private ControllerState interactState = new ControllerState(StateID.Interact);
        public ControllerState InteractState => interactState;
#pragma warning restore IDE0044
        #endregion

        #region Private Field
        private readonly List<ControllerState> defaultStates = new List<ControllerState>();

        private XRBaseController baseController = null;
        private XRBaseInteractor baseInteractor = null;
        private XRInteractorLineVisual baseLineVisual = null;

        private XRBaseController teleportController = null;
        private XRBaseInteractor teleportInteractor = null;
        private XRInteractorLineVisual teleportLineVisual = null;
        #endregion

        protected void OnEnable()
        {
            FindBaseControllerComponents();
            FindTeleportControllerComponents();

            selectState.OnEnter.AddListener(OnEnterSelectState);
            selectState.OnUpdate.AddListener(OnUpdateSelectState);
            selectState.OnExit.AddListener(OnExitSelectState);

            teleportState.OnEnter.AddListener(OnEnterTeleportState);
            teleportState.OnUpdate.AddListener(OnUpdateTeleportState);
            teleportState.OnExit.AddListener(OnExitTeleportState);

            interactState.OnEnter.AddListener(OnEnterInteractState);
            interactState.OnUpdate.AddListener(OnUpdateInteractState);
            interactState.OnExit.AddListener(OnExitInteractState);
        }

        protected void OnDisable()
        {
            selectState.OnEnter.RemoveListener(OnEnterSelectState);
            selectState.OnUpdate.RemoveListener(OnUpdateSelectState);
            selectState.OnExit.RemoveListener(OnExitSelectState);

            teleportState.OnEnter.RemoveListener(OnEnterTeleportState);
            teleportState.OnUpdate.RemoveListener(OnUpdateTeleportState);
            teleportState.OnExit.RemoveListener(OnExitTeleportState);

            interactState.OnEnter.RemoveListener(OnEnterInteractState);
            interactState.OnUpdate.RemoveListener(OnUpdateInteractState);
            interactState.OnExit.RemoveListener(OnExitInteractState);
        }

        protected void Start()
        {
            defaultStates.Add(selectState);
            defaultStates.Add(teleportState);
            defaultStates.Add(interactState);

            TransitionState(null, selectState);
        }

        protected void Update()
        {
            foreach (var state in defaultStates)
            {
                if (state.Enabled)
                {
                    state.OnUpdate.Invoke();
                    return;
                }
            }
        }

        #region Find Components
        private void FindBaseControllerComponents()
        {
            if (baseControllerGameObject == null)
            {
                Debug.LogWarning("Base controller GameObject is missing.");
                return;
            }

            if (baseController == null)
            {
                baseController = baseControllerGameObject.GetComponent<XRBaseController>();
                if (baseController == null)
                    Debug.Log($"Cannot find any {nameof(XRBaseController)} component on the Base Controller GameObject.", this);
            }
            if (baseInteractor == null)
            {
                baseInteractor = baseControllerGameObject.GetComponent<XRBaseInteractor>();
                if (baseInteractor == null)
                    Debug.LogWarning($"Cannot find any {nameof(XRBaseInteractor)} component on the Base Controller GameObject.", this);
            }
            if (baseInteractor is XRRayInteractor && baseLineVisual == null)
            {
                baseLineVisual = baseControllerGameObject.GetComponent<XRInteractorLineVisual>();
                if (baseLineVisual == null)
                    Debug.LogWarning($"Cannot find any {nameof(XRInteractorLineVisual)} component on the Base Controller GameObject.", this);
            }
        }

        private void FindTeleportControllerComponents()
        {
            if (teleportControllerGameObject == null)
            {
                Debug.LogWarning("Teleport controller GameObject is missing.");
                return;
            }

            if (teleportController == null)
            {
                teleportController = teleportControllerGameObject.GetComponent<XRBaseController>();
                if (teleportController == null)
                    Debug.Log($"Cannot find any {nameof(XRBaseController)} component on the Base Controller GameObject.", this);
            }
            if (teleportInteractor == null)
            {
                teleportInteractor = teleportControllerGameObject.GetComponent<XRBaseInteractor>();
                if (teleportInteractor == null)
                    Debug.LogWarning($"Cannot find any {nameof(XRBaseInteractor)} component on the Base Controller GameObject.", this);
            }
            if (teleportInteractor == null)
            {
                teleportLineVisual = teleportControllerGameObject.GetComponent<XRInteractorLineVisual>();
                if (teleportLineVisual == null)
                    Debug.LogWarning($"Cannot find any {nameof(XRInteractorLineVisual)} component on the Base Controller GameObject.", this);
            }
        }
        #endregion

        #region Set Controllers
        /// <summary>
        /// Find and configure the components on the base controller.
        /// </summary>
        private void SetBaseController(bool enable)
        {
            FindBaseControllerComponents();

            if (baseController != null)
                baseController.enableInputActions = enable;
            if (baseInteractor != null)
                baseInteractor.enabled = enable;
            if ( baseInteractor is XRRayInteractor && baseLineVisual != null)
                baseLineVisual.enabled = enable;
        }

        /// <summary>
        /// Find and configure the components on the teleport controller.
        /// </summary>
        private void SetTeleportController(bool enable)
        {
            FindTeleportControllerComponents();

            if (teleportController != null)
                teleportController.enableInputActions = enable;
            if (teleportInteractor != null)
                teleportInteractor.enabled = enable;
            if (teleportLineVisual != null)
                teleportLineVisual.enabled = enable;
        }
        #endregion

        #region Select & Exit State
        private void OnEnterSelectState(StateID previousStateID)
        {
            switch (previousStateID)
            {
                case StateID.None:
                    // Enable transitions to Teleport state.
                    EnableAction(teleportModeActivate);
                    EnableAction(teleportModeCancel);

                    // Enable turn and move actions.
                    EnableAction(turn);
                    EnableAction(move);

                    // Enable base controller components
                    SetBaseController(true);
                    break;

                case StateID.Select:
                    break;

                case StateID.Teleport:
                    EnableAction(turn);
                    EnableAction(move);
                    SetBaseController(true);
                    break;

                case StateID.Interact:
                    EnableAction(turn);
                    EnableAction(move);
                    break;

                default:
                    Debug.Assert(false, $"Unhandled case when entering Select from {previousStateID}.");
                    break;
            }
        }

        private void OnExitSelectState(StateID nextStateID)
        {
            switch (nextStateID)
            {
                case StateID.None:
                    break;

                case StateID.Select:
                    break;

                case StateID.Teleport:
                    DisableAction(turn);
                    DisableAction(move);
                    SetBaseController(false);
                    break;

                case StateID.Interact:
                    DisableAction(turn);
                    DisableAction(move);
                    break;

                default:
                    Debug.Assert(false, $"Unhandled case when exiting Select to {nextStateID}.");
                    break;
            }
        }

        private void OnEnterTeleportState(StateID previousStateID) => SetTeleportController(true);

        private void OnExitTeleportState(StateID nextStateID) => SetTeleportController(false);

        private void OnEnterInteractState(StateID previousStateID)
        {
            EnableAction(translateAnchor);
            EnableAction(rotateAnchor);
        }

        private void OnExitInteractState(StateID nextStateID)
        {
            DisableAction(translateAnchor);
            DisableAction(rotateAnchor);
        }

        private static void EnableAction(InputActionReference actionReference)
        {
            InputAction action = GetInputAction(actionReference);
            if (action != null && !action.enabled)
                action.Enable();
        }

        private static void DisableAction(InputActionReference actionReference)
        {
            InputAction action = GetInputAction(actionReference);
            if (action != null && action.enabled)
                action.Disable();
        }

        private static InputAction GetInputAction(InputActionReference actionReference)
        {
#pragma warning disable IDE0031 // Use null propagation -- Do not use for UnityEngine.Object types
            return actionReference != null ? actionReference.action : null;
#pragma warning restore IDE0031
        }
        #endregion

        #region Update State
        /// <summary>
        /// This method is automatically called each frame to handle initiating transitions out of the Select state.
        /// </summary>
        private void OnUpdateSelectState()
        {
            // Transition from Select state to Teleport state when the user triggers the "Teleport Mode Activate" action but not the "Cancel Teleport" action
            InputAction teleportModeAction = GetInputAction(teleportModeActivate);
            InputAction cancelTelportModeAction = GetInputAction(teleportModeCancel);

            bool triggerTeleportMode = teleportModeAction != null && teleportModeAction.triggered;
            bool cancelTeleport = cancelTelportModeAction != null && cancelTelportModeAction.triggered;

            if (triggerTeleportMode && !cancelTeleport)
            {
                TransitionState(selectState, teleportState);
                return;
            }

            // Transition from Select state to Interact state when the interactor has a selectTarget
            FindBaseControllerComponents();

            if (baseInteractor.selectTarget != null)
                TransitionState(selectState, interactState);
        }

        /// <summary>
        /// Updated every frame to handle the transition to selectState state.
        /// </summary>
        private void OnUpdateTeleportState()
        {
            // Transition from Teleport state to Select state when we release the Teleport trigger or cancel Teleport mode
            InputAction teleportModeAction = GetInputAction(teleportModeActivate);
            InputAction cancelTeleportModeAction = GetInputAction(teleportModeCancel);

            bool cancelTeleport = cancelTeleportModeAction != null && cancelTeleportModeAction.triggered;
            bool releasedTeleport = teleportModeAction != null && teleportModeAction.phase == InputActionPhase.Waiting;

            if (cancelTeleport || releasedTeleport)
                TransitionState(teleportState, selectState);
        }

        private void OnUpdateInteractState()
        {
            // Transition from Interact state to Select state when the base interactor no longer has a select target
            if (baseInteractor.selectTarget == null)
                TransitionState(interactState, selectState);
        }

        private void TransitionState(ControllerState fromState, ControllerState toState)
        {
            if (fromState != null)
            {
                fromState.Enabled = false;
                fromState.OnExit.Invoke(toState?.ID ?? StateID.None);
            }

            if (toState != null)
            {
                toState.OnEnter.Invoke(fromState?.ID ?? StateID.None);
                toState.Enabled = true;
            }
        }
        #endregion

        public enum StateID { None, Select, Teleport, Interact, }

        [Serializable]
        public class StateEnterEvent : UnityEvent<StateID> { }

        [Serializable]
        public class StateUpdateEvent : UnityEvent { }

        [Serializable]
        public class StateExitEvent : UnityEvent<StateID> { }

        [Serializable]
        public class ControllerState
        {
            [Tooltip("Sets the controller state to be active.")]
            [SerializeField] private bool enabled = false;
            public bool Enabled
            {
                get => enabled;
                set => enabled = value;
            }

            [HideInInspector]
            [SerializeField] private StateID id;
            public StateID ID
            {
                get => id;
                set => id = value;
            }

            [Tooltip("Event that will be invoked when entering the controller state.")]
            [SerializeField] private StateEnterEvent onEnter = new StateEnterEvent();
            public StateEnterEvent OnEnter
            {
                get => onEnter;
                set => onEnter = value;
            }

            [Tooltip("Event that will be invoked when updating the controller state.")]
            [SerializeField] private StateUpdateEvent onUpdate = new StateUpdateEvent();
            public StateUpdateEvent OnUpdate
            {
                get => onUpdate;
                set => onUpdate = value;
            }

            [Tooltip("Event that will be invoked when exiting the controller state.")]
            [SerializeField] private StateExitEvent onExit = new StateExitEvent();
            public StateExitEvent OnExit
            {
                get => onExit;
                set => onExit = value;
            }

            public ControllerState(StateID defaultId = StateID.None) => this.ID = defaultId;
        }
    }
}
