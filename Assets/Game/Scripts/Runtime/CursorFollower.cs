using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    enum Mode { Screen, World }
    [SerializeField] Mode mode;

    void Update()
    {
        Vector2 pos = mode == Mode.Screen ? InputManager.CursorScreenPosition : InputManager.CursorWorldPosition;
        transform.position = pos;
    }
}
