using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    readonly Dictionary<ItemData, int> items = new();

    public void AddItem(ItemData item)
    {
        if (items.ContainsKey(item)) items[item]++;
        else items.Add(item, 1);
        item.OnAmountChanged?.Invoke(items[item]);
    }

    public void RemoveItem(ItemData item)
    {
        if (items.ContainsKey(item) && items[item] > 0)
        {
            items[item]--;
            item.OnAmountChanged?.Invoke(items[item]);
        }
    }
}
