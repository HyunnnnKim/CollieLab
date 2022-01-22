using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieLab.XR.Interactables
{
    public class Arrow : MonoBehaviour
    {
        #region Serialize Field
        #endregion

        #region Private Field
        private Rigidbody body = null;
        public Rigidbody Body
        {
            get => body;
        }
        #endregion

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }
    }
}
