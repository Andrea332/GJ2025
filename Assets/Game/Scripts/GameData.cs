using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static void Init(Inventory inventory)
    {
        Inventory = inventory;
        ClearAllData();
    }

    static readonly HashSet<string> unlockedLocksIds = new();

    public static Inventory Inventory { get; private set; }

    public static bool IsLockUnlocked(string id) => unlockedLocksIds.Contains(id);

    public static void SaveUnlockedLock(string id)
    {
        if (!unlockedLocksIds.Contains(id)) unlockedLocksIds.Add(id);
    }

    public static void ClearAllData()
    {
        unlockedLocksIds.Clear();
        Inventory.ClearAll();
    }
}
