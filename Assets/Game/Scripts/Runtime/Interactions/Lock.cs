using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Lock : Interactable
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] ItemData requiredItem;
    [SerializeField] bool removeItems;
    [SerializeField] bool automatic;
    [Space]
    [SerializeField] Inventory inventory;
    [SerializeField] CoroutineAnimation unlockAnimation;
    [SerializeField] UnityEvent onUnlock;

    public ItemData RequiredItem => requiredItem;
    public bool Unlocked { get; private set; }

    void OnEnable()
    {
        if (automatic && !Unlocked)
        {
            OnInteract(Vector2.zero);
        }
    }

    protected override void OnInteract(Vector2 worldInteractPosition)
    {
        if (Unlocked) return;
        if (inventory && inventory.HasItem(requiredItem))
        {
            Unlocked = true;
            if (removeItems) inventory.RemoveItem(requiredItem);
            if (unlockAnimation) unlockAnimation.Play(gameObject, onUnlock.Invoke);
            else onUnlock.Invoke();
        }
    }
}
