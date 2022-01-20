using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class PhysicsHand : PhysicsTracker
    {
        #region Serialized Field
        [Header("Push Interaction")]
        [SerializeField] private float pushForce = 200f;
        [SerializeField] private float pushDrag = 30f;
        #endregion
        
        #region Private Field
        private Rigidbody body = null;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            body = GetComponent<Rigidbody>();
            if (body == null)
                Debug.Log($"Couldn't find any {nameof(body)} component on the Physics Hand.", this);
        }

        private void FixedUpdate()
        {
            PerformPhysicsTracking(body);
            //PushInteraction();
        }

        private void Update()
        {
            SwitchTrackingAutomatic();
            PerformNonPhysicsTracking(body);
        }

        #region Physics Interaction
        /// <summary>
        /// Achieved using Hook's Law.
        /// Hook's law states Force = Stiffness * Delta from Rest.
        /// </summary>
        private void PushInteraction()
        {
            // trigger condition needed.

            Vector3 deltaFromResting = transform.position - target.position;
            Vector3 force = deltaFromResting * pushForce;
            Vector3 drag = -playerBody.velocity * pushDrag;
            // Applying drag will bring spring to rest.

            playerBody.AddForce(force, ForceMode.Acceleration);
            playerBody.AddForce(drag, ForceMode.Acceleration);
        }
        #endregion
    }
}
