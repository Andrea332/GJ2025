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
            var cursor = value ? cursorOverride : defaultCursor;
            Cursor.SetCursor(cursor, new Vector2(cursor.width * 0.5f, cursor.height * 0.5f), CursorMode.Auto);
        }
        OnHover(value, worldInteractPosition);
    }

    protected virtual void OnInteract(Vector2 worldInteractPosition) { }
    protected virtual void OnHover(bool value, Vector2 worldInteractPosition) { }
}
