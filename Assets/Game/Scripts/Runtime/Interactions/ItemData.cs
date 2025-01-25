using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "GGJ/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] string id;

    [SerializeField] Sprite inGameSprite;

    public string Id => id;
    public Sprite InGameSprite => inGameSprite;
}
