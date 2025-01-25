using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] ItemData item;
    [Space]
    [SerializeField] Inventory inventory;
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI countText;

    void OnEnable()
    {
        if (!item) return;
        if (iconImage) iconImage.sprite = item.InventorySprite;

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
        if (countText) countText.text = quantity.ToString();
    }

    void OnValidate()
    {
        if (iconImage && item) iconImage.sprite = item.InventorySprite;
    }
}
