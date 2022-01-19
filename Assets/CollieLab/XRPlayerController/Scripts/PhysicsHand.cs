using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class PhysicsHand : MonoBehaviour
    {
        #region Serialized Field
        [Header("References")]
        [SerializeField] private Rigidbody playerBody = null;
        [SerializeField] private Transform targetController = null;
        [SerializeField] private Collider[] ignoreColliders = null;

        [Header("Position Settings")]
        [SerializeField] private float positionFrequency = 50f;
        [SerializeField] private float positionDamping = 1f;

        [Header("Rotation Settings")]
        [SerializeField] private float rotationFrequency = 100f;
        [SerializeField] private float rotationDamping = 1f;

        [Header("Push Interaction")]
        [SerializeField] private float pushForce = 200f;
        [SerializeField] private float pushDrag = 30f;
        #endregion
        
        #region Private Field
        private Rigidbody body = null;
        private bool isPhysicallyInteracting = false;
        #endregion

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            if (body == null)
                Debug.Log($"Couldn't find any {nameof(body)} component on the Physics Hand.", this);

            IgnoreCollisions();
        }

        #region Initialize
        private void IgnoreCollisions()
        {
            if (ignoreColliders == null) return;
            Collider handCollider = GetComponent<Collider>();
            for (int i = 0; i < ignoreColliders.Length; i++)
                Physics.IgnoreCollision(handCollider, ignoreColliders[i]);
        }
        #endregion

        private void FixedUpdate()
        {
            PositionTracking();
            RotationTracking();
            PushInteraction();
        }

        #region Physics Tracking
        /// <summary>
        /// Using position and velocity.
        /// </summary>
        private void PositionTracking()
        {
            PIDController(positionFrequency, positionDamping, out float ksg, out float kdg);
            Vector3 force = (targetController.position - transform.position) * ksg + (playerBody.velocity - body.velocity) * kdg;
            body.AddForce(force, ForceMode.Acceleration);
        }

        /// <summary>
        /// Using rotation and angular velocity.
        /// </summary>
        private void RotationTracking()
        {
            PIDController(rotationFrequency, rotationDamping, out float ksg, out float kdg);
            Quaternion targetRotation = targetController.rotation * Quaternion.Inverse(transform.rotation);
            if (targetRotation.w < 0)
            {
                targetRotation.x = -targetRotation.x;
                targetRotation.y = -targetRotation.y;
                targetRotation.z = -targetRotation.z;
                targetRotation.w = -targetRotation.w;
            }
            targetRotation.ToAngleAxis(out float angle, out Vector3 axis);
            axis.Normalize();
            axis *= Mathf.Deg2Rad;
            Vector3 torque = ksg * axis * angle + -body.angularVelocity * kdg;
            body.AddTorque(torque, ForceMode.Acceleration);
        }

        /// <summary>
        /// 
        /// </summary>
        private void PIDController(float frequency, float damping, out float ksg, out float kdg)
        {
            float kp = (6f * frequency) * (6f * frequency) * 0.25f;
            float kd = 4.5f * frequency * damping;
            float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
            ksg = kp * g;
            kdg = (kd + kp * Time.fixedDeltaTime) * g;
        }
        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            isPhysicallyInteracting = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            isPhysicallyInteracting = false;
        }

        #region Physics Interaction
        /// <summary>
        /// Achieved using Hook's Law.
        /// Hook's law states Force = Stiffness * Delta from Rest.
        /// </summary>
        private void PushInteraction()
        {
            if (!isPhysicallyInteracting) return;
            Vector3 deltaFromResting = transform.position - targetController.position;
            Vector3 force = deltaFromResting * pushForce;
            Vector3 drag = -playerBody.velocity * pushDrag;
            // Applying drag will bring spring to rest.

            playerBody.AddForce(force, ForceMode.Acceleration);
            playerBody.AddForce(drag, ForceMode.Acceleration);
        }
        #endregion
    }
}
