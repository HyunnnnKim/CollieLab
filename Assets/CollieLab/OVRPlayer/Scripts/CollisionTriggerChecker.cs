using CollieLab.Assets;
using UnityEngine;

namespace CollieLab.Sensors
{
    public class CollisionTriggerChecker : MonoBehaviour
    {
        #region Serialized Field
        [SerializeField] private Collider colliderForUse = null;
        [SerializeField] private CheckType selectedCheckType = CheckType.Collision;
        [SerializeField] private CheckBoudaryType selectedBoundary = CheckBoudaryType.Sphere;
        [SerializeField] private float sphereRadius = 0.3f;
        [SerializeField] private Vector3 boxSize = Vector3.one;
        [SerializeField] private CheckCount selectedType = CheckCount.Single;
        [SerializeField] private IgnoreGameObjects ignoreGameObjects = null;
        #endregion

        #region Private Field
        private GameObject triggerer = null;
        private bool isTriggered = false;
        public bool IsTriggered
        {
            get => isTriggered;
            set => isTriggered = value;
        }
        #endregion

        private void Awake()
        {
            AddColliderIfNeeded();
        }

        #region Initialize
        /// <summary>
        /// Only adds a collider if it doesn't have one.
        /// </summary>
        private void AddColliderIfNeeded()
        {
            if (colliderForUse != null) return;

            switch (selectedBoundary)
            {
                case CheckBoudaryType.Sphere:
                    SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
                    sphere.radius = sphereRadius;
                    sphere.isTrigger = selectedCheckType == CheckType.Trigger;
                    break;

                case CheckBoudaryType.Box:
                    BoxCollider box = gameObject.AddComponent<BoxCollider>();
                    box.size = boxSize;
                    box.isTrigger = selectedCheckType == CheckType.Trigger;
                    break;
            }
        }
        #endregion

        private void OnCollisionEnter(Collision collision)
        {
            if (selectedCheckType != CheckType.Collision) return;

        }

        private void OnCollisionExit(Collision collision)
        {
            if (selectedCheckType != CheckType.Collision) return;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (selectedCheckType != CheckType.Trigger) return;

            switch (selectedType)
            {
                case CheckCount.Single:
                    if (this.triggerer != null) break;

                    GameObject triggerer = other.gameObject;
                    if (ignoreGameObjects != null)
                    {
                        if (ignoreGameObjects.Has(triggerer)) break;
                    }

                    this.triggerer = triggerer;
                    isTriggered = true;
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (selectedCheckType != CheckType.Trigger) return;

            switch (selectedType)
            {
                case CheckCount.Single:
                    if (triggerer != other.gameObject) break;

                    isTriggered = false;
                    triggerer = null;
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            if (colliderForUse == null) return;

            Gizmos.color = Color.yellow;
            if (colliderForUse is SphereCollider sphere)
            {
                Gizmos.DrawWireSphere(transform.position, sphere.radius);
            }
            else if (colliderForUse is BoxCollider box)
            {
                Gizmos.DrawWireCube(transform.position, box.size);
            }
        }

        #region Enums
        public enum CheckType { Collision, Trigger }
        public enum CheckBoudaryType { Sphere, Box }
        public enum CheckCount { Single, Multiple }
        #endregion
    }
}
