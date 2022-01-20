using UnityEngine;

namespace CollieLab.Helper
{
    public static class TrackingHelper
    {
        #region Physics Tracking
        /// <summary>
        /// Using the difference in position for proportional value and
        /// diffence in velocity for derivative value.
        /// </summary>
        public static void TrackPositionPID(this Rigidbody body, Vector3 targetPos, Vector3 rootVelocity, float frequency, float damping)
        {
            PIDController(frequency, damping, out float ksg, out float kdg);
            Vector3 proportionalValue = targetPos - body.position;
            Vector3 derivativeValue = rootVelocity - body.velocity;
            Vector3 force = proportionalValue * ksg + derivativeValue * kdg;
            body.AddForce(force, ForceMode.Acceleration);
        }

        /// <summary>
        /// Using difference in rotation and angular velocity.
        /// </summary>
        public static void TrackRotationPID(this Rigidbody body, Quaternion targetRot, float frequency, float damping)
        {
            PIDController(frequency, damping, out float ksg, out float kdg);
            Quaternion targetRotation = CalculationHelper.ShortestRotation(targetRot, body.rotation);
            targetRotation.ToAngleAxis(out float angle, out Vector3 axis);
            axis.Normalize();
            axis *= Mathf.Deg2Rad;
            Vector3 torque = axis * angle * ksg + -body.angularVelocity * kdg;
            body.AddTorque(torque, ForceMode.Acceleration);
        }

        private static void PIDController(float frequency, float damping, out float ksg, out float kdg)
        {
            float kp = (6f * frequency) * (6f * frequency) * 0.25f;
            float kd = 4.5f * frequency * damping;
            float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
            ksg = kp * g;
            kdg = (kd + kp * Time.fixedDeltaTime) * g;
        }

        /// <summary>
        /// Position tracking using velocity.
        /// </summary>
        public static void TrackPositionVelocity(this Rigidbody body, Vector3 targetPos, float slowDownVel, float maxPosChange)
        {
            body.velocity *= slowDownVel;

            Vector3 targetVel = targetPos - body.position;
            targetVel /= Time.fixedDeltaTime;

            if (IsValidVelocity(targetVel.x))
            {
                float maxChange = maxPosChange * Time.fixedDeltaTime;
                body.velocity = Vector3.MoveTowards(body.velocity, targetVel, maxChange);
            }
        }

        /// <summary>
        /// Rotation tracking using angular velocity.
        /// </summary>
        public static void TrackRotationVelocity(this Rigidbody body, Quaternion targetRot, float slowDownAngularVel, float maxRotChange)
        {
            body.angularVelocity *= slowDownAngularVel;

            Quaternion rot = targetRot * Quaternion.Inverse(body.rotation);
            rot.ToAngleAxis(out float angleInDegrees, out Vector3 rotationAxis);
            if (angleInDegrees > 180)
                angleInDegrees -= 360;
            Vector3 angularVel = (rotationAxis * angleInDegrees * Mathf.Deg2Rad) / Time.fixedDeltaTime;

            if (IsValidVelocity(angularVel.x))
            {
                float maxChange = maxRotChange * Time.fixedDeltaTime;
                body.angularVelocity = Vector3.MoveTowards(body.angularVelocity, angularVel, maxChange);
            }
        }

        private static bool IsValidVelocity(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }
        #endregion

        #region Non-Physics Tracking
        /// <summary>
        /// Position tracking using Transform position directly of a physics object.
        /// </summary>
        public static void TrackPositionTransform(this Rigidbody body, Vector3 targetPos)
        {
            body.velocity = Vector3.zero;
            body.transform.position = targetPos;
        }

        /// <summary>
        /// Rotation tracking using Transform rotation directly of a physics object.
        /// </summary>
        public static void TrackRotationTransform(this Rigidbody body, Quaternion targetRot)
        {
            body.angularVelocity = Vector3.zero;
            body.transform.rotation = targetRot;
        }
        #endregion
    }
}
