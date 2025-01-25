using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GGJ/Inventory")]
public class Inventory : ScriptableObject
{
    [ShowInInspector, ReadOnly]
    readonly Dictionary<ItemData, int> items = new();

    public event Action<ItemData, int> ItemAmountChanged;

    public bool HasItem(ItemData item) => GetItemAmount(item) > 0;

    public int GetItemAmount(ItemData item) => items.ContainsKey(item) ? items[item] : 0;

    public void AddItem(ItemData item)
    {
        if (items.ContainsKey(item)) items[item]++;
        else items.Add(item, 1);
        ItemAmountChanged?.Invoke(item, items[item]);
    }

    public void RemoveItem(ItemData item)
    {
        if (items.ContainsKey(item) && items[item] > 0)
        {
            items[item]--;
            ItemAmountChanged?.Invoke(item, items[item]);
        }
    }
}
