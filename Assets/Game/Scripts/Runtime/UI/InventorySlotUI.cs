using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] ItemData item;
    [Space]
    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI countText;

    public static Dictionary<string, InventorySlotUI> registeredSlots = new();

    void Awake()
    {
        registeredSlots.TryAdd(item.Id, this);
    }

    void OnEnable()
    {
        if (!item) return;
        if (!inventory) return;
        inventory.ItemAmountChanged += OnAmountChanged;
        if (countText) countText.text = inventory.GetItemAmount(item).ToString();
    }

    void OnDisable()
    {
        if (!inventory) return;
        inventory.ItemAmountChanged -= OnAmountChanged;
    }

    public void OnAmountChanged(ItemData item, int quantity)
    {
        if (item.Id != this.item.Id) return;
        if (countText) countText.text = quantity.ToString();
    }
}
