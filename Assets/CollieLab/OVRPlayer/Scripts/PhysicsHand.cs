using System;
using CollieLab.XR.Interactables;
using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class PhysicsHand : PhysicsTracker
    {
        #region Events
        public Action<XRGrabbable> OnDetectGrabbable = null;
        public Action<XRGrabbable> OnPassOverGrabbable = null;
        public Action<XRGrabbable> OnGrabGrabbable = null;
        public Action OnReleaseGrabbable = null;
        #endregion

        #region Serialized Field
        [Header("Hand Settings")]
        [SerializeField] private Hand selectedHand = Hand.Left;

        [Header("Grab & Climb")]
        [SerializeField] private Transform rayOrigin = null;
        [SerializeField] private float rayRadius = 0.03f;
        [SerializeField] private float rayLength = 0.02f;
        [SerializeField] private float dropDistance = 0.6f;
        [SerializeField] private LayerMask grabbableLayer = new LayerMask();
        [SerializeField] private LayerMask climbableLayer = new LayerMask();
        [SerializeField] private bool colorChangeOnDetection = true;

        [Header("Push")]
        [SerializeField] private float pushForce = 200f;
        [SerializeField] private float pushDrag = 30f;
        #endregion
        
        #region Private Field
        private Rigidbody body = null;
        public Rigidbody Body
        {
            get => body;
        }

        private Collider handCollider = null;
        public Collider HandCollider
        {
            get => handCollider;
        }

        private GameObject grabbableGameObject = null;
        private XRGrabbable grabbable = null;
        private XRGrabbable grabbed = null;
        private Joint grabbedJoint = null;
        private bool isGrabReady = false;
        private bool grabbableDetected = false;
        private float dstFromGrabbed = 0f;
        private bool grabInput = false;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            body = GetComponent<Rigidbody>();
            if (body == null)
                Debug.Log($"Couldn't find any {nameof(body)} component on the Physics Hand.", this);

            handCollider = GetComponent<Collider>();
            if (handCollider == null)
            {
                Debug.Log($"Couldn't find any {nameof(handCollider)} component on the Physics Hand.", this);
                return;
            }
            IgnoreCollisions(handCollider);
        }

        private void FixedUpdate()
        {
            PerformPhysicsPostionTracking(body);
            PerformPhysicsRotationTracking(body);

            //PushInteraction();
            PerformPhysicsGrab();
        }

        private void Update()
        {
            AutomaticTrackingMode();
            PerformNonPhysicsPostionTracking(body);
            PerformNonPhysicsRotationTracking(body);

            ReadControllerInput();
            DetectGrabbable();
            PerformDrop();
        }

        #region Manage Input
        /// <summary>
        /// Read controller input for physics hand.
        /// </summary>
        private void ReadControllerInput()
        {
            if (selectedHand == Hand.Left)
            {
                grabInput = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch);
            }
            else if (selectedHand == Hand.Right)
            {
                grabInput = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.RTouch);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DetectGrabbable()
        {
            if (grabInput) return;
            Detect();

            void Detect()
            {
                Vector3 rayDir = rayOrigin.TransformDirection(Vector3.forward);
                Ray ray = new Ray(transform.position, rayDir);
                if (Physics.SphereCast(ray, rayRadius, out var hit, rayLength, grabbableLayer))
                {
                    GameObject hitObject = hit.transform.gameObject;
                    if (grabbableGameObject == hitObject) return;

                    XRGrabbable grabbable = hitObject.GetComponent<XRGrabbable>();
                    if (grabbable == null)
                    {
                        grabbable = hitObject.GetComponentInParent<XRGrabbable>();
                        if (grabbable == null)
                            Debug.LogWarning($"Couldn't find any {nameof(grabbable)} component on the {hitObject.name}.");
                        return;
                    }

                    if (colorChangeOnDetection)
                    {
                        if (this.grabbable != null && !this.grabbable.IsGrabbing)
                            ResetColorOfGrabbable(this.grabbable);
                        if (!grabbable.IsGrabbing)
                            ChangeColorOfGrabbable(grabbable, Color.yellow);
                    }

                    this.grabbable = grabbable;
                    grabbableGameObject = hitObject;
                    grabbableDetected = true;
                    OnDetectGrabbable?.Invoke(this.grabbable);
                }
                else
                {
                    if (!grabbableDetected) return;

                    if (colorChangeOnDetection)
                    {
                        if (!this.grabbable.IsGrabbing)
                            ResetColorOfGrabbable(grabbable);
                    }

                    if (grabbable != null)
                        grabbable = null;
                    if (grabbableGameObject != null)
                        grabbableGameObject = null;

                    grabbableDetected = false;
                    OnPassOverGrabbable?.Invoke(grabbable);
                }
            }
        }

        private void AutomaticTrackingMode()
        {
            if (grabbed != null)
            {
                GrabTracking();
                return;
            }

            if (collisionTriggerChecker.IsTriggered)
            {
                PhysicsTracking();
                //TransformTracking();
            }
            else
            {
                TransformTracking();
            }

            void GrabTracking()
            {
                selectedPosTracking = TrackingMode.PIDController;
                selectedRotTracking = TrackingMode.PIDController;
            }

            void PhysicsTracking()
            {
                selectedPosTracking = TrackingMode.PIDController;
                selectedRotTracking = TrackingMode.PIDController;
            }

            void TransformTracking()
            {
                selectedPosTracking = TrackingMode.Transform;
                selectedRotTracking = TrackingMode.Transform;
            }
        }
        #endregion

        #region Physics Interaction
        /// <summary>
        /// Achieved using Hook's Law.
        /// Hook's law states Force = Stiffness * Delta from Rest.
        /// </summary>
        private void PushInteraction()
        {
            if (grabbed == null) return;

            Vector3 deltaFromResting = transform.position - targetTracker.position;
            Vector3 force = deltaFromResting * pushForce;
            Vector3 drag = -physicsPlayer.HoverBody.velocity * pushDrag;
            // Applying drag will bring spring to rest.

            physicsPlayer.HoverBody.AddForce(force, ForceMode.Acceleration);
            physicsPlayer.HoverBody.AddForce(drag, ForceMode.Acceleration);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PerformPhysicsGrab()
        {
            if (grabInput)
            {
                if (!isGrabReady) return;
                isGrabReady = false;

                if (grabbed != null) return;
                PhysicsGrab();
            }
            else
            {
                isGrabReady = true;
                if (grabbed != null)
                {
                    ReleaseGrabbed();
                }
            }

            void PhysicsGrab()
            {
                grabbedJoint = grabbable.FixedJointGrab(this);
                //grabbedJoint = grabbable.ConfigurableJointGrab(this);
                if (grabbedJoint != null)
                {
                    grabbed = grabbable;
                    dstFromGrabbed = Vector3.Distance(grabbed.transform.position, transform.position);

                    if (colorChangeOnDetection)
                        ChangeColorOfGrabbable(grabbed, Color.green);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PerformDrop()
        {
            if (grabbed == null) return;

            float dst = Vector3.Distance(grabbed.transform.position, transform.position);
            if (Mathf.Abs(dst - dstFromGrabbed) > dropDistance)
                ReleaseGrabbed();
        }

        private void ReleaseGrabbed()
        {
            grabbed.Release(grabbedJoint);
            if (colorChangeOnDetection)
            {
                if (!grabbed.IsGrabbing)
                    ResetColorOfGrabbable(grabbed);
            }
            grabbed = null;
            dstFromGrabbed = 0f;
        }

        private void ChangeColorOfGrabbable(XRGrabbable grabbable, Color color)
        {
            if (grabbable == null) return;
            grabbable.ChangeColor(color);
        }

        private void ResetColorOfGrabbable(XRGrabbable grabbable)
        {
            if (grabbable == null) return;
            grabbable.ResetColor();
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 forwardDir = rayOrigin.TransformDirection(Vector3.forward);
            Gizmos.DrawRay(transform.position, forwardDir * rayLength);
            Vector3 pos = transform.position + (forwardDir * (rayLength - rayRadius));
            Gizmos.DrawWireSphere(pos, rayRadius);
        }

        public enum Hand { Left, Right }
    }
}
