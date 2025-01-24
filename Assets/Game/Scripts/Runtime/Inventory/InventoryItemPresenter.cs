using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class InventoryItemPresenter : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Button button;
        public Button Button => button;
        private ItemData itemData;
        public ItemData ItemData
        {
            get => itemData;
            set
            {
                itemData = value;
                SetItemSprite(itemData.InventorySprite);
            }
        }
        public int ItemId { get; set; }
        public UnityAction<InventoryItemPresenter> OnClick;
        private GameObject inventoryPanel;
        public GameObject InventoryPanel
        {
            get => inventoryPanel;
            set
            {
                inventoryPanel = value;
            }
        }

        private void SetItemSprite(Sprite sprite)
        {
            itemImage.sprite = sprite;
        }

        public void OnButtonClick()
        {
            OnClick?.Invoke(this);
        }
    }
}
