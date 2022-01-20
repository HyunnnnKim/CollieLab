using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class PhysicsHead : PhysicsTracker
    {
        #region Private Field
        private Rigidbody body = null;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            body = GetComponent<Rigidbody>();
            if (body == null)
                Debug.Log($"Couldn't find any {nameof(body)} component on the Physics Head.", this);
        }

        private void FixedUpdate()
        {
            PerformPhysicsTracking(body);
        }

        private void Update()
        {
            SwitchTrackingAutomatic();
            PerformNonPhysicsTracking(body);
        }
    }
}
