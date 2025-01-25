using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] Texture2D cursorOverride;
    [SerializeField] Texture2D defaultCursor;
    [SerializeField] UnityEvent onInteract;

    public void Interact(Vector2 worldInteractPosition)
    {
        OnInteract(worldInteractPosition);
        onInteract.Invoke();
    }

    public void Hover(bool value, Vector2 worldInteractPosition)
    {
        if (cursorOverride)
        {
            Cursor.SetCursor(value ? cursorOverride : defaultCursor, Vector2.zero, CursorMode.Auto);
        }
        OnHover(value, worldInteractPosition);
    }

    protected virtual void OnInteract(Vector2 worldInteractPosition) { }
    protected virtual void OnHover(bool value, Vector2 worldInteractPosition) { }
}
