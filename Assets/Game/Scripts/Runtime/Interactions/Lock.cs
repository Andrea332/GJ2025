using System;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class Lock : Interactable
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] ItemData requiredItem;
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] string lockId;
    [SerializeField] int requiredItemAmount = 1;
    [SerializeField] bool removeItems;
    [SerializeField] bool automatic;
    [Space]
    [SerializeField] Prsd_AudioSet unlockAudio;
    [SerializeField] Prsd_AudioSet lockedAudio;
    [SerializeField] CoroutineAnimation unlockAnimation;
    [SerializeField] UnityEvent onUnlock;
    [SerializeField] UnityEvent onUnlockFail;

    public ItemData RequiredItem => requiredItem;

    public bool Unlocked => GameData.IsLockUnlocked(lockId);

    public event Action LockUnlocked;

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
        var inventory = GameData.Inventory;
        if (inventory && inventory.GetItemAmount(requiredItem) >= requiredItemAmount)
        {
            GameData.SaveUnlockedLock(lockId);
            if (removeItems)
            {
                for (int i = 0; i < requiredItemAmount; i++) inventory.RemoveItem(requiredItem);
            }
            if (unlockAnimation) unlockAnimation.Play(gameObject, Unlock);
            else Unlock();
        }
        else
        {
            if (lockedAudio) Prsd_AudioManager.PlaySoundSet(lockedAudio);
            onUnlockFail.Invoke();
        }
    }

    void Unlock()
    {
        if (unlockAudio) Prsd_AudioManager.PlaySoundSet(unlockAudio);
        OnUnlock();
    }

    void OnUnlock()
    {
        unlockTriggered = true;
        LockUnlocked?.Invoke();
        onUnlock.Invoke();
        gameObject.SetActive(false);
    }
}
