using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollieLab.XR.Controllers
{
    public class TrackingSpaceController : MonoBehaviour
    {
        #region Serialized Field
        [Header("References")]
        [SerializeField] private Transform targetBody = null;
        #endregion

        private void Update()
        {
            transform.position = targetBody.position;
            transform.rotation = targetBody.rotation;
        }
    }
}
