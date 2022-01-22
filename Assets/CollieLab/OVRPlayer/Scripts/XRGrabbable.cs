using System.Collections.Generic;
using CollieLab.XR.Controllers;
using UnityEngine;

namespace CollieLab.XR.Interactables
{
    public class XRGrabbable : MonoBehaviour
    {
        #region Serialize Field
        [SerializeField] private int grabHandLimit = 1;

        #endregion

        #region Private Field
        private int currentGrabCount = 0;
        private bool isGrabbable = true;
        private bool isGrabbing = false;
        public bool IsGrabbing
        {
            get => isGrabbing;
        }

        private Renderer[] grabbableRenderer = null;
        private Color[] originalColors;
        #endregion

        private void Awake()
        {
            grabbableRenderer = GetComponentsInChildren<Renderer>();
            List<Color> originalColorList = new List<Color>();
            for (int i = 0; i < grabbableRenderer.Length; i++)
            {
                originalColorList.Add(grabbableRenderer[i].material.color);
            }
            originalColors = originalColorList.ToArray();
        }

        private void OnJointBreak(float breakForce)
        {
            currentGrabCount = currentGrabCount > 0 ? currentGrabCount -= 1 : 0;
            isGrabbable = currentGrabCount < grabHandLimit;
            if (currentGrabCount == 0)
            {
                ResetColor();
            }
        }

        #region Physics Interactions
        /// <summary>
        /// 
        /// </summary>
        public FixedJoint FixedJointGrab(PhysicsHand hand)
        {
            if (!isGrabbable) return null;
            isGrabbing = true;
            currentGrabCount++;
            isGrabbable = currentGrabCount < grabHandLimit;
            return CreateJoint();

            FixedJoint CreateJoint()
            {
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = hand.Body;
                joint.enablePreprocessing = false;
                return joint;
            }
        }

        public ConfigurableJoint ConfigurableJointGrab(PhysicsHand hand)
        {
            if (!isGrabbable) return null;
            isGrabbing = true;
            currentGrabCount++;
            isGrabbable = currentGrabCount < grabHandLimit;
            return CreateJoint();

            ConfigurableJoint CreateJoint()
            {
                ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
                joint.connectedBody = hand.Body;
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
                joint.angularXMotion = ConfigurableJointMotion.Locked;
                joint.angularYMotion = ConfigurableJointMotion.Locked;
                joint.angularZMotion = ConfigurableJointMotion.Locked;
                return joint;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Release(Joint joint)
        {
            currentGrabCount--;
            if (!isGrabbable)
                isGrabbable = true;

            if (currentGrabCount == 0)
                isGrabbing = false;

            if (joint != null)
                Destroy(joint);
        }
        #endregion

        #region
        public void ChangeColor(Color color)
        {
            if (grabbableRenderer == null) return;
            for (int i = 0; i < grabbableRenderer.Length; i++)
            {
                grabbableRenderer[i].material.color = color;
            }
        }

        public void ResetColor()
        {
            if (grabbableRenderer == null) return;
            for (int i = 0; i < grabbableRenderer.Length; i++)
            {
                grabbableRenderer[i].material.color = originalColors[i];
            }
        }
        #endregion
    }
}
