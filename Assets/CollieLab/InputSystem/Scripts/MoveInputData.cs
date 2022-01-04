using UnityEngine;

namespace CollieLab.Inputs
{
    [CreateAssetMenu(fileName = "MoveInputData", menuName = "CollieLab/MoveInputData")]
    public class MoveInputData : ScriptableObject
    {
        public Vector2 movement = Vector2.zero;
        public bool HasMovement => movement != Vector2.zero;

        public bool runKeyDown = false;
        public bool runKeyUp = false;
        public bool runKeyPressed = false;

        public bool jumpKeyDown = false;
        public bool jumpKeyUp = false;
        public bool jumpKeyPressed = false;
    }
}
