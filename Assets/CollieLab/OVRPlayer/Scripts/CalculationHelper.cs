using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieLab.Helper
{
    public static class CalculationHelper
    {
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
