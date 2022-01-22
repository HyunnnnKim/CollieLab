using UnityEngine;

namespace CollieLab.Interactables
{
    public class CenterOfMassModifier : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField] private Rigidbody body = null;
        [SerializeField] private Vector3 centerOfMass = Vector3.zero;
        #endregion

        private void Awake()
        {
            if (body == null)
                GetComponent<Rigidbody>();
        }

        private void Update()
        {
            body.centerOfMass = centerOfMass;
            body.WakeUp();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.rotation * centerOfMass, 0.1f);
        }
    }
}
