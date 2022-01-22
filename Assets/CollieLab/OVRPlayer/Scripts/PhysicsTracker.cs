using CollieLab.Helper;
using CollieLab.Sensors;
using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class PhysicsTracker : MonoBehaviour
    {
        #region Serialized Field
        [Header("References")]
        [SerializeField] protected PhysicsPlayerController physicsPlayer = null;
        [SerializeField] protected Transform targetTracker = null;

        [Header("Tracking Settings")]
        [SerializeField] protected TrackingMode selectedPosTracking = TrackingMode.PIDController;
        [SerializeField] protected TrackingMode selectedRotTracking = TrackingMode.PIDController;
        [SerializeField] protected CollisionTriggerChecker collisionTriggerChecker = null;

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

        #region Protected Field
        protected Rigidbody trackerBody = null;
        protected ConfigurableJoint joint = null;
        #endregion

        protected virtual void Awake()
        {
            if (physicsPlayer == null)
                physicsPlayer = GetComponentInParent<PhysicsPlayerController>();
        }

        #region Initialize
        // Needs modification.
        protected void IgnoreCollisions(Collider collider)
        {
            if (physicsPlayer.PlayerIgnoreObjects == null) return;

            Collider[] ignoreList = physicsPlayer.PlayerIgnoreObjects.Colliders;
            for (int i = 0; i < ignoreList.Length; i++)
                Physics.IgnoreCollision(collider, ignoreList[i]);
        }
        #endregion

        #region Tracking
        protected void SetPositionTrackingMode(TrackingMode mode)
        {
            if (selectedPosTracking != mode)
                selectedPosTracking = mode;
        }

        protected void SetRotationTrackingMode(TrackingMode mode)
        {
            if (selectedRotTracking != mode)
                selectedRotTracking = mode;
        }

        protected void PerformPhysicsPostionTracking(Rigidbody body)
        {
            switch (selectedPosTracking)
            {
                case TrackingMode.PIDController:
                    body.TrackPositionPID(targetTracker.position, physicsPlayer.HoverBody.velocity, positionFrequency, positionDamping);
                    break;

                case TrackingMode.Velocity:
                    body.TrackPositionVelocity(targetTracker.position, slowDownVel, maxPosChange);
                    break;
            }
        }

        protected void PerformNonPhysicsPostionTracking(Rigidbody body)
        {
            switch (selectedPosTracking)
            {
                case TrackingMode.Transform:
                    body.TrackPositionTransform(targetTracker.position);
                    break;
            }
        }

        protected void PerformPhysicsRotationTracking(Rigidbody body)
        {
            switch (selectedPosTracking)
            {
                case TrackingMode.PIDController:
                    body.TrackRotationPID(targetTracker.rotation, rotationFrequency, rotationDamping);
                    break;

                case TrackingMode.Velocity:
                    body.TrackRotationVelocity(targetTracker.rotation, slowDownAngularVel, maxRotChange);
                    break;
            }
        }

        protected void PerformNonPhysicsRotationTracking(Rigidbody body)
        {
            switch (selectedPosTracking)
            {
                case TrackingMode.Transform:
                    body.TrackRotationTransform(targetTracker.rotation);
                    break;
            }
        }
        #endregion

        public enum TrackingMode { PIDController, Velocity, Transform }
    }
}
