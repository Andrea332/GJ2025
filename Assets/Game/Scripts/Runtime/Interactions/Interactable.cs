using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;

    public void Interact(Vector2 worldInteractPosition)
    {
        OnInteract(worldInteractPosition);
        onInteract.Invoke();
    }

    protected virtual void OnInteract(Vector2 worldInteractPosition) { }
}
