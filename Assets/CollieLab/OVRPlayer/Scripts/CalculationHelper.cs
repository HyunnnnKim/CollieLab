using UnityEngine;
using static CollieLab.Interactables.CompoundBody;

namespace CollieLab.Helper
{
    public static class CalculationHelper
    {
        #region Rigidbody
        public static Vector3 CenterOfMass(this Rigidbody[] rigidbodies)
        {
            Vector3 com = Vector3.zero;
            float sum = 0f;

            for (int i = 0; i < rigidbodies.Length; i++)
            {
                Rigidbody body = rigidbodies[i];
                com += body.position * body.mass;
                sum += body.mass;
            }
            com /= sum;
            return com;
        }

        public static Vector3 CenterOfMass(this Compound[] compounds)
        {
            Vector3 com = Vector3.zero;
            float sum = 0f;

            for (int i = 0; i < compounds.Length; i++)
            {
                Compound compound = compounds[i];
                com += compound.transform.localPosition * compound.mass;
                sum += compound.mass;
            }
            com /= sum;
            return com;
        }
        #endregion

        #region Rotation
        public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
        {
            return Quaternion.Dot(a, b) < 0 ? a * Quaternion.Inverse(Multiply(b, -1)) : a * Quaternion.Inverse(b);
        }

        public static Quaternion Multiply(Quaternion input, float scalar)
        {
            return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
        }
        #endregion
    }
}
