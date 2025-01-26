using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lock : Interactable
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] ItemData requiredItem;
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] string lockId;
    [SerializeField] int requiredItemAmount = 1;
    [SerializeField] bool removeItems;
    [SerializeField] bool automatic;
    [SerializeField] string[] partitionsToVisit;
    [Space]
    [SerializeField] Inventory inventory;
    [SerializeField] CoroutineAnimation unlockAnimation;
    [SerializeField] UnityEvent onUnlock;

    public ItemData RequiredItem => requiredItem;

    public bool Unlocked => unlockedIds.Contains(lockId);

    public event Action LockUnlocked;

    static readonly HashSet<string> unlockedIds = new();

    bool unlockTriggered;


    void OnEnable()
    {
        if (!unlockTriggered && Unlocked) OnUnlock();
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
            unlockedIds.Add(lockId);
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
        unlockTriggered = true;
        LockUnlocked?.Invoke();
        onUnlock.Invoke();
    }
}
