using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInputHandler : ScriptableObject
{
    public virtual void HandleInput(XRController controller) { }
}
