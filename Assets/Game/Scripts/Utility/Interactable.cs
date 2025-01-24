using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour, IInteractAble
{
    public UnityEvent<Vector3> onInteract;

    public void Interact(Vector3 position)
    {
        if (!enabled) return;
        onInteract.Invoke(position);
    }
}
