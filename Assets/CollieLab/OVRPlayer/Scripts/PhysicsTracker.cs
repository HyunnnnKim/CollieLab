using CollieLab.Helper;
using CollieLab.Sensors;
using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class PhysicsTracker : MonoBehaviour
    {
        #region Serialized Field
        [Header("References")]
        [SerializeField] protected Rigidbody playerBody = null;
        [SerializeField] protected Transform target = null;
        [SerializeField] protected Collider[] ignoreColliders = null;

        [Header("Tracking Settings")]
        [SerializeField] protected TrackingMethod selectedTracking = TrackingMethod.PIDController;
        [SerializeField] protected bool useAutoTracking = true;
        [SerializeField] private CheckTrigger checkTrigger = null;

        [Header("PID Tracking")]
        [SerializeField] protected float positionFrequency = 50f;
        [SerializeField] protected float positionDamping = 1f;
        [SerializeField] protected float rotationFrequency = 100f;
        [SerializeField] protected float rotationDamping = 1f;

        [Header("Velocity Tracking")]
        [Range(0, 1)] [SerializeField] protected float slowDownVel = 0.75f;
        [Range(0, 1)] [SerializeField] protected float slowDownAngularVel = 0.75f;
        [Range(0, 100)] [SerializeField] protected float maxPosChange = 75f;
        [Range(0, 100)] [SerializeField] protected float maxRotChange = 75f;
        #endregion

        protected virtual void Awake()
        {
            IgnoreCollisions();
        }

        #region Initialize
        private void IgnoreCollisions()
        {
            if (ignoreColliders == null) return;
            Collider collider = GetComponent<Collider>();
            if (collider == null)
            {
                Debug.Log($"Couldn't find any {nameof(collider)} component on the Physics Tracker.", this);
                return;
            }

            for (int i = 0; i < ignoreColliders.Length; i++)
                Physics.IgnoreCollision(collider, ignoreColliders[i]);
        }
        #endregion

        #region Tracking
        /// <summary>
        /// Switch tracking method if collision is detected.
        /// </summary>
        protected void SwitchTrackingAutomatic()
        {
            if (!useAutoTracking) return;

            if (checkTrigger == null)
            {
                Debug.Log($"Couldn't find any {nameof(checkTrigger)} component on the Physics Tracker.", this);
                return;
            }

            if (checkTrigger.IsTriggered)
            {
                if (selectedTracking != TrackingMethod.PIDController)
                    selectedTracking = TrackingMethod.PIDController;
            }
            else
            {
                if (selectedTracking != TrackingMethod.Transform)
                    selectedTracking = TrackingMethod.Transform;
            }
        }

        /// <summary>
        /// Perform physics tracking.
        /// </summary>
        protected void PerformPhysicsTracking(Rigidbody body)
        {
            switch (selectedTracking)
            {
                case TrackingMethod.PIDController:
                    body.TrackPositionPID(target.position, playerBody.velocity, positionFrequency, positionDamping);
                    body.TrackRotationPID(target.rotation, rotationFrequency, rotationDamping);
                    break;

                case TrackingMethod.Velocity:
                    body.TrackPositionVelocity(target.position, slowDownVel, maxPosChange);
                    body.TrackRotationVelocity(target.rotation, slowDownAngularVel, maxRotChange);
                    break;
            }
        }

        /// <summary>
        /// Perform non-physics tracking.
        /// Should be called on
        /// </summary>
        protected void PerformNonPhysicsTracking(Rigidbody body)
        {
            switch (selectedTracking)
            {
                case TrackingMethod.Transform:
                    body.TrackPositionTransform(target.position);
                    body.TrackRotationTransform(target.rotation);
                    break;
            }
        }
        #endregion

        public enum TrackingMethod { PIDController, Velocity, Transform }
    }
}
