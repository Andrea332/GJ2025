using Sirenix.OdinInspector;
using UnityEngine;

public class Item : Interactable
{
    [GUIColor(0.8f, 0.8f, 1.4f)]
    [SerializeField] ItemData itemData;
    [Space]
    [SerializeField] CoroutineAnimation pickAnimation;
    [SerializeField] SpriteRenderer spriteRenderer;

    public ItemData ItemData => itemData;

    protected override void OnInteract(Vector2 worldInteractPosition)
    {
        if (pickAnimation) pickAnimation.Play(gameObject, AddToInventory);
        else AddToInventory();
        gameObject.SetActive(false);
    }

    void AddToInventory()
    {
        GameData.Inventory.AddItem(itemData);
    }

    void OnValidate()
    {
        if (itemData && spriteRenderer) spriteRenderer.sprite = itemData.InGameSprite;
    }

    void Start()
    {
        if (!itemData) Debug.LogError("Missing ItemData from item: " + name);
    }
}
