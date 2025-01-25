using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ/Inventory")]
public class Inventory : ScriptableObject
{
    [ShowInInspector, ReadOnly]
    readonly Dictionary<string, int> items = new();

    public event Action<ItemData, int> ItemAmountChanged;

    public bool HasItem(ItemData item) => GetItemAmount(item) > 0;

    public int GetItemAmount(ItemData item) => items.ContainsKey(item.Id) ? items[item.Id] : 0;

    public void AddItem(ItemData item)
    {
        if (items.ContainsKey(item.Id)) items[item.Id]++;
        else items.Add(item.Id, 1);
        ItemAmountChanged?.Invoke(item, items[item.Id]);
    }

    public void RemoveItem(ItemData item)
    {
        if (items.ContainsKey(item.Id) && items[item.Id] > 0)
        {
            items[item.Id]--;
            ItemAmountChanged?.Invoke(item, items[item.Id]);
        }
    }
}
