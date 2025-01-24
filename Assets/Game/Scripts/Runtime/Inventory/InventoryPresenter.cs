using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private DataEventItem dataEventItemAdded;
        [SerializeField] private DataEventItem dataEventItemRemoved;
        [SerializeField] private InventoryItemPresenter inventoryItemPresenterPrefab;
        [SerializeField] private Transform  itemPresenterParent;
        [SerializeField] private Transform  itemClickedBackground;
        [SerializeField] private Transform  itemClickedParent;
        private readonly List<InventoryItemPresenter> _itemPresentersSpawned = new ();
        private GameObject itemClickedSpawnedObject;
        private InventoryItemPresenter currentItemPresenter;
        private void Awake()
        {
            DeactivateItemClickedObject();
            dataEventItemAdded.OnDataChanged.AddListener(OnItemAdded);
            dataEventItemRemoved.OnDataChanged.AddListener(OnItemRemoved);
        }

        private void OnItemAdded(Item itemAdded)
        {
            var itemPresenterSpawned = Instantiate(inventoryItemPresenterPrefab, itemPresenterParent);
            itemPresenterSpawned.ItemData = itemAdded.ItemData;
            itemPresenterSpawned.ItemId = itemAdded.GetInstanceID();
            itemPresenterSpawned.OnClick += OpenItem;
            itemPresenterSpawned.InventoryPanel = Instantiate(itemPresenterSpawned.ItemData.ItemClickedOnInventoryPrefab, itemClickedParent);
            //itemPresenterSpawned.InventoryPanel.GetComponent<CanvasGroup>().alpha = 0;
            _itemPresentersSpawned.Add(itemPresenterSpawned);
        }

        private void OnItemRemoved(Item itemRemoved)
        {
            var itemPresenterToRemove = _itemPresentersSpawned.Find(itemPresenter => itemPresenter.ItemId == itemRemoved.GetInstanceID());
            _itemPresentersSpawned.Remove(itemPresenterToRemove);
            Destroy(itemPresenterToRemove.gameObject);
        }

        private void DeactivateItemClickedObject()
        {
            itemClickedBackground.gameObject.SetActive(false);
        }
        private void ActivateItemClickedObject()
        {
            itemClickedBackground.gameObject.SetActive(true);
        }
        
        private void OpenItem(InventoryItemPresenter inventoryItemPresenter)
        {
            ActivateItemClickedObject();
            //inventoryItemPresenter.InventoryPanel.GetComponent<CanvasGroup>().alpha = 1;
            currentItemPresenter = inventoryItemPresenter;
            inventoryItemPresenter.ItemData.onItemSelectedOnInventory.RaiseEvent();
        }

        public void CloseItem()
        {
            currentItemPresenter.ItemData.onItemClosedOnInventory.RaiseEvent();
            //currentItemPresenter.InventoryPanel.GetComponent<CanvasGroup>().alpha = 0;
            DeactivateItemClickedObject();
        }
    }
}
