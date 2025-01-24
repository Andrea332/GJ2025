using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "GGJ/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] Sprite inGameSprite;
    [SerializeField] Sprite inventorySprite;

    public Sprite InventorySprite => inventorySprite;

    public Action<int> OnAmountChanged { get; set; }
}
