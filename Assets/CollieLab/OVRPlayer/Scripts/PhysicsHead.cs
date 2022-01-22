using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class PhysicsHead : PhysicsTracker
    {
        #region Private Field
        private Rigidbody body = null;
        private Collider headCollider = null;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            body = GetComponent<Rigidbody>();
            if (body == null)
                Debug.Log($"Couldn't find any {nameof(body)} component on the Physics Head.", this);

            headCollider = GetComponent<Collider>();
            if (headCollider == null)
            {
                Debug.Log($"Couldn't find any {nameof(headCollider)} component on the Physics Hand.", this);
                return;
            }
            IgnoreCollisions(headCollider);
        }

        private void FixedUpdate()
        {
            PerformPhysicsPostionTracking(body);
            PerformPhysicsRotationTracking(body);
        }

        private void Update()
        {
            AutomaticTrackingMode();
            PerformNonPhysicsPostionTracking(body);
            PerformNonPhysicsRotationTracking(body);
        }

        private void AutomaticTrackingMode()
        {
            if (collisionTriggerChecker.IsTriggered)
            {
                selectedPosTracking = TrackingMode.PIDController;
                selectedRotTracking = TrackingMode.PIDController;
            }
            else
            {
                selectedPosTracking = TrackingMode.Transform;
                selectedRotTracking = TrackingMode.Transform;
            }
        }
    }
}
