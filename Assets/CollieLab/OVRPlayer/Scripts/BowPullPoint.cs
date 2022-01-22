using CollieLab.Helper;
using UnityEngine;

namespace CollieLab.XR.Interactables
{
    public class BowPullPoint : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField] private LayerMask targetLayer = new LayerMask();
        [SerializeField] private float attachAngle = 45f;
        #endregion

        #region Private Field
        private Arrow selectedArrow = null;
        private bool isAttached = false;
        private bool inAngle = false;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (targetLayer != (1 << other.gameObject.layer) || isAttached) return;

            Arrow arrow = other.gameObject.GetComponent<Arrow>();
            if (arrow == null) return;

            inAngle = arrow.transform.forward.InAngle(transform.forward, attachAngle);
            if (inAngle)
            {
                AttachBow();
            }

            void AttachBow()
            {
                isAttached = true;
                selectedArrow = arrow;

                Rigidbody body = gameObject.AddComponent<Rigidbody>();
                body.useGravity = false;
                body.interpolation = RigidbodyInterpolation.Interpolate;

                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = arrow.Body;
                //joint.enablePreprocessing = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!isAttached) return;


        }
    }
}
