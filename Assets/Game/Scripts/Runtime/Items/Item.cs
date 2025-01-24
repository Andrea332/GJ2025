using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ItemData itemData;

    public ItemData ItemData => itemData;
}
