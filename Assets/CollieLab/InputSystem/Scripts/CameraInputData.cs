using UnityEngine;

namespace CollieLab.Inputs
{
    [CreateAssetMenu(fileName = "CameraInputData", menuName = "CollieLab/CameraInputData")]
    public class CameraInputData : ScriptableObject
    {
        public Vector2 cameraMovement = Vector2.zero;
        public bool HasCameraMovement => cameraMovement != Vector2.zero;
    }
}
