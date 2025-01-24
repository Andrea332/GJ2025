using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.DebugComponents
{
    public class DebugInputCallback : DebugComponent<InputAction.CallbackContext>
    {
        public void ShowDebugVector2(InputAction.CallbackContext context)
        {
            Debug.Log(context.ReadValue<Vector2>());
        }
    }
}
