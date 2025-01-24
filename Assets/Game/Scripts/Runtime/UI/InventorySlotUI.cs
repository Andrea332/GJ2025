using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] ItemData item;
    [Space]
    [SerializeField] Image iconImage;
    [SerializeField] TextMeshProUGUI countText;

    void OnEnable()
    {
        if (!item) return;
        if (iconImage) iconImage.sprite = item.InventorySprite;
        item.OnAmountChanged += OnAmountChanged;
    }

    void OnDisable()
    {
        if (!item) return;
        item.OnAmountChanged -= OnAmountChanged;
    }

    public void OnAmountChanged(int quantity)
    {
        if (countText) countText.text = quantity.ToString();
    }
}
