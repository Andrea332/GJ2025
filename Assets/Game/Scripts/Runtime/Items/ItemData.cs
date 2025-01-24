using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Game
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Objects/Item Data")]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private Sprite inGameSprite;
        [SerializeField] private Sprite inventorySprite;
        [SerializeField] private GameObject itemClickedOnInventoryPrefab;
        [SerializeField] private VisualTreeAsset inventoryPage;

        public VisualTreeAsset InventoryPage
        {
            get => inventoryPage;
            set => inventoryPage = value;
        }

        public SimpleEvent onItemSelectedOnInventory;
        public SimpleEvent onItemClosedOnInventory;
        public Sprite InGameSprite
        {
            get => inGameSprite;
            set => inGameSprite = value;
        }

        public Sprite InventorySprite
        {
            get => inventorySprite;
            set => inventorySprite = value;
        }

        public GameObject ItemClickedOnInventoryPrefab
        {
            get => itemClickedOnInventoryPrefab;
            set => itemClickedOnInventoryPrefab = value;
        }
    }
}
