using System.Collections.Generic;
using CollieLab.Helper;
using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class PhysicsPlayerController : MonoBehaviour
    {
        #region Serialized Field
        [Header("References")]
        [SerializeField] private Rigidbody physicsBody = null;

        [Header("Float")]
        [SerializeField] private float targetHeight = 1f;
        [SerializeField] private float springStrength = 500f;
        [SerializeField] private float springDamper = 200f;
        [SerializeField] private float rayLength = 1.3f;
        [SerializeField] private List<Transform> ignoreGameObjects = null;

        [Header("UpRight")]
        [SerializeField] private float uprightFrequency = 1000f;
        [SerializeField] private float uprightDrag = 20f;

        [Header("Movement")]
        [SerializeField] private float walkSpeed = 130f;
        [SerializeField] private float runSpeed = 230;
        [SerializeField] private float acceleration = 100f;
        [SerializeField] private float maxAccelForce = 250f;

        [Header("Turn")]
        [SerializeField] private bool snapTurnEnabled = true;
        [SerializeField] private float snapTurnAngle = 45f;
        [SerializeField] private float rotSpeed = 20f;

        [Header("Jump")]
        [SerializeField] private float jumpUpVel = 23f;
        [SerializeField] private bool enableMovementInAir = false;
        #endregion

        #region Private Field
        private OVRCameraRig cameraRig = null;

        private Rigidbody hitBody = null;
        private Vector3 inputDir = Vector3.zero;
        private Vector3 targetVel = Vector3.zero;
        private bool onGround = true;
        private bool snapTurnReady = true;

        private bool runInput = false;
        private bool jumpInput = false;
        #endregion

        private void Awake()
        {
            InitOVRCameraRig();
        }

        private void Start()
        {
            SetCameraOffset();
        }

        #region Initialize
        private void InitOVRCameraRig()
        {
            OVRCameraRig[] cameraRigs = gameObject.GetComponentsInChildren<OVRCameraRig>();

            if (cameraRigs.Length == 0)
                Debug.LogWarning("[PhysicsPlayerController] No OVRCameraRig attached.");
            else if (cameraRigs.Length > 1)
                Debug.LogWarning("[PhysicsPlayerController] More then 1 OVRCameraRig attached.");
            else
                cameraRig = cameraRigs[0];
        }

        private void SetCameraOffset()
        {
            var p = cameraRig.transform.localPosition;
            p.z = OVRManager.profile.eyeDepth;
            cameraRig.transform.localPosition = p;

            p = cameraRig.transform.localPosition;
            if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.EyeLevel)
            {
                p.y = OVRManager.profile.eyeHeight;
            }
            else if (OVRManager.instance.trackingOriginType == OVRManager.TrackingOrigin.FloorLevel)
            {
                p.y = 1.8f;
            }
            cameraRig.transform.localPosition = p;
        }
        #endregion

        private void FixedUpdate()
        {
            FloatBody();
            UprightBody();
            Movement();
            Jump();
        }

        private void Update()
        {
            ReadInput();
            Turn();
        }

        #region Input
        private void ReadInput()
        {
            Vector2 primaryAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
            inputDir = cameraRig.transform.right * primaryAxis.x + cameraRig.transform.forward * primaryAxis.y;

            runInput = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);
            jumpInput = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch);
        }
        #endregion

        #region Float Upright Calculation
        /// <summary>
        /// Enable physics body to float.
        /// </summary>
        private void FloatBody()
        {
            Vector3 downDir = physicsBody.transform.TransformDirection(Vector3.down);
            RaycastHit[] hits = Physics.RaycastAll(physicsBody.transform.position, downDir, rayLength);
            if (hits.Length > 0)
            {
                if (!onGround)
                    onGround = true;

                for (int i = 0; i < hits.Length; i++)
                {
                    if (ignoreGameObjects.Contains(hits[i].transform)) continue;

                    Vector3 hitVel = Vector3.zero;
                    hitBody = hits[i].rigidbody;
                    if (hitBody != null)
                        hitVel = hitBody.velocity;

                    float bodyDirVel = Vector3.Dot(downDir, physicsBody.velocity);
                    float hitDirVel = Vector3.Dot(downDir, hitVel);
                    float relitiveVel = bodyDirVel - hitDirVel;

                    float targetDst = hits[i].distance - targetHeight;
                    float springForce = (targetDst * springStrength) - (relitiveVel * springDamper);
                    physicsBody.AddForce(downDir * springForce);

                    if (hitBody != null)
                        hitBody.AddForceAtPosition(downDir * -springForce, hits[i].point);
                    break;
                }
            }
            else
            {
                if (onGround)
                    onGround = false;

                if (hitBody != null)
                    hitBody = null;
            }
        }

        /// <summary>
        /// Stops the physics body from falling..
        /// </summary>
        private void UprightBody()
        {
            Vector3 forward = physicsBody.transform.TransformDirection(Vector3.up);
            physicsBody.TrackRotationPID(Quaternion.Euler(forward), uprightFrequency, uprightDrag);
        }
        #endregion

        #region Locomotion
        /// <summary>
        /// Physics based movement.
        /// </summary>
        private void Movement()
        {
            if (enableMovementInAir ? false : !onGround) return;

            Vector3 groundVel = hitBody == null ? Vector3.zero : hitBody.velocity;
            float targetSpeed = runInput ? runSpeed : walkSpeed;

            Vector3 goalVel = inputDir * targetSpeed * Time.fixedDeltaTime;
            targetVel = Vector3.MoveTowards(targetVel, (goalVel + groundVel), acceleration * Time.fixedDeltaTime);
            Vector3 targetAccel = (targetVel - physicsBody.velocity) / Time.fixedDeltaTime;
            targetAccel = Vector3.ClampMagnitude(targetAccel, maxAccelForce);
            physicsBody.AddForce(Vector3.Scale(targetAccel * physicsBody.mass, new Vector3(1f, 0f, 1f)));
        }

        /// <summary>
        /// Rotate the CameraRig.
        /// </summary>
        private void Turn()
        {
            if (snapTurnEnabled)
            {
                Vector3 playerForward = cameraRig.transform.rotation.eulerAngles;
                if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch))
                {
                    if (snapTurnReady)
                    {
                        playerForward.y -= snapTurnAngle;
                        snapTurnReady = false;
                    }
                }
                else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch))
                {
                    if (snapTurnReady)
                    {
                        playerForward.y += snapTurnAngle;
                        snapTurnReady = false;
                    }
                }
                else
                {
                    snapTurnReady = true;
                }
                cameraRig.transform.rotation = Quaternion.Euler(playerForward);
            }
            else
            {
                // Smooth turn goes here
            }
        }

        /// <summary>
        /// Perform jump only when player is on ground.
        /// </summary>
        private void Jump()
        {
            if (jumpInput && onGround)
            {
                physicsBody.AddForce(Vector3.up * jumpUpVel, ForceMode.Impulse);
            }
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(physicsBody.transform.position, 0.3f);
        }
    }
}
