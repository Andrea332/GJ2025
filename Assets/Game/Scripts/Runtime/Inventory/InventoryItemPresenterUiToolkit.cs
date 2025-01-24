using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class InventoryItemPresenterUiToolkit
    {
        private ItemData itemData;
        public ItemData ItemData
        {
            get => itemData;
            set
            {
                itemData = value;
            }
        }
        public int ItemId { get; set; }
        public GameObject InventoryPanel { get; set; }
    }
}
