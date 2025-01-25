using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;

public class Lock : Interactable
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] ItemData requiredItem;
    [SerializeField] int requiredItemAmount = 1;
    [SerializeField] bool removeItems;
    [SerializeField] bool automatic;
    [SerializeField] string[] partitionsToVisit;
    [Space]
    [SerializeField] Inventory inventory;
    [SerializeField] CoroutineAnimation unlockAnimation;
    [SerializeField] UnityEvent onUnlock;

    public ItemData RequiredItem => requiredItem;

    public bool Unlocked { get; private set; }

    public event Action LockUnlocked;

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
        for (int i = 0; i < partitionsToVisit.Length; i++)
        {
            if (!WorldManager.HasVisitedPartition(partitionsToVisit[i])) return;
        }
        if (inventory && inventory.GetItemAmount(requiredItem) >= requiredItemAmount)
        {
            Unlocked = true;
            if (removeItems)
            {
                for (int i = 0; i < requiredItemAmount; i++) inventory.RemoveItem(requiredItem);
            }
            if (unlockAnimation) unlockAnimation.Play(gameObject, OnUnlock);
            else OnUnlock();
        }
    }

    void OnUnlock()
    {
        LockUnlocked?.Invoke();
        onUnlock.Invoke();
    }
}
