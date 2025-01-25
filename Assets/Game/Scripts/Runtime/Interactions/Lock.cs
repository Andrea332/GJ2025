using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Lock : Interactable
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] ItemData requiredItem;
    [Space]
    [SerializeField] Inventory inventory;
    [SerializeField] UnityEvent onUnlock;

    protected override void OnInteract(Vector2 worldInteractPosition)
    {
        if (inventory && inventory.HasItem(requiredItem))
        {
            inventory.RemoveItem(requiredItem);
            onUnlock.Invoke();
        }
    }
}
