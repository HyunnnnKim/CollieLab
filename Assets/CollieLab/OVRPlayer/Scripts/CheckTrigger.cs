using System.Collections.Generic;
using UnityEngine;

namespace CollieLab.Sensors
{
    public class CheckTrigger : MonoBehaviour
    {
        #region Serialized Field
        [SerializeField] private SphereCollider triggerCollider = null;
        [SerializeField] private List<Transform> ignoreGameObjects = null;
        #endregion

        #region Private Field
        private bool isTriggered = false;
        public bool IsTriggered
        {
            get => isTriggered;
            set => isTriggered = value;
        }

        private Transform triggeredObject = null;
        #endregion

        private void Awake()
        {
            if (!triggerCollider.isTrigger)
                triggerCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsTriggered) return;
            if (ignoreGameObjects.Contains(other.transform)) return;

            isTriggered = true;
            triggeredObject = other.transform;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform != triggeredObject) return;

            isTriggered = false;
        }

        private void OnDrawGizmos()
        {
            if (triggerCollider == null) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, triggerCollider.radius);
        }
    }
}
