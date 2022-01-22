using System;
using System.Collections.Generic;
using CollieLab.Helper;
using UnityEngine;

namespace CollieLab.Interactables
{
    public class CompoundBody : MonoBehaviour
    {
        #region Private Field
        private Rigidbody compoundBody = null;
        private Compound[] compounds = null;
        #endregion

        private void Awake()
        {
            InitCompoundBody();
        }

        #region Initialize
        private void InitCompoundBody()
        {
            Rigidbody[] compoundBodies = GetComponentsInChildren<Rigidbody>();
            List<Compound> compoundsList = new List<Compound>();
            float compoundMass = 0f;
            for (int i = 0; i < compoundBodies.Length; i++)
            {
                Rigidbody body = compoundBodies[i];
                compoundMass += body.mass;
                compoundsList.Add(new Compound() { transform = body.transform, mass = body.mass });
                Destroy(body);
            }
            compounds = compoundsList.ToArray();

            compoundBody = gameObject.AddComponent<Rigidbody>();
            compoundBody.mass = compoundMass;
            compoundBody.centerOfMass = compounds.CenterOfMass();
            compoundBody.interpolation = RigidbodyInterpolation.Interpolate;
            compoundBody.WakeUp();
        }
        #endregion

        private void Update()
        {
            compoundBody.centerOfMass = compounds.CenterOfMass();
            compoundBody.WakeUp();
        }

        private void OnDrawGizmos()
        {
            if (compoundBody == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(compoundBody.position + compoundBody.rotation * compoundBody.centerOfMass, 0.1f);
        }

        [Serializable]
        public class Compound
        {
            public Transform transform = null;
            public float mass = 0f;
        }
    }
}
