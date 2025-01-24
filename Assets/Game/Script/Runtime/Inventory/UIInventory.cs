using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace Game
{
    public class UIInventory : MonoBehaviour
    {
        [SerializeField] private DataEventItem dataEventItemAdded;
        [SerializeField] private DataEventItem dataEventItemRemoved;
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private VisualTreeAsset inventoryItemVisualTreeAsset;
        [SerializeField] private InputActionReference interactWithObject;
        private readonly List<InventoryItemPresenterUiToolkit> _itemPresentersSpawned = new ();
        
        private VisualElement _inventoryItemPage;
        private VisualElement _inventoryItemPageContainer;
        private VisualElement _inventoryItemPageSpawned;
        private InventoryItemPresenterUiToolkit _currentInventoryItemPresenter;
        private Button _inventoryItemPageExitButton;
        private GameObject _itemClickedSpawnedObject;

        /*private void Awake()
        {
            _inventoryItemPage = uiDocument.rootVisualElement.Q("InventoryItemPage");
            _inventoryItemPageContainer =  _inventoryItemPage.Q("Container");
            _inventoryItemPageExitButton = _inventoryItemPage.Q<Button>("ExitFromInventoryPageButton");
            dataEventItemAdded.OnDataChanged.AddListener(OnItemAdded);
            dataEventItemRemoved.OnDataChanged.AddListener(OnItemRemoved);
        }*/

        private void OnItemAdded(Item itemAdded)
        {
            var itemPresenterSpawned = new InventoryItemPresenterUiToolkit
            {
                ItemData = itemAdded.ItemData,
                ItemId = itemAdded.GetInstanceID(),
                InventoryPanel = Instantiate(itemAdded.ItemData.ItemClickedOnInventoryPrefab, transform)
            };
            var inventoryItemsVisualElement = uiDocument.rootVisualElement.Q<ScrollView>("InventoryItems");
            var templateContainer = inventoryItemVisualTreeAsset.CloneTree();
            inventoryItemsVisualElement.Add(templateContainer);
            var button = templateContainer.Q<Button>("InventoryItemButton");
            button.style.backgroundImage = new StyleBackground(itemAdded.ItemData.InventorySprite);
            button.RegisterCallback<ClickEvent>(_ => OpenItem(itemPresenterSpawned));
            _itemPresentersSpawned.Add(itemPresenterSpawned);
        }

        private void OnItemRemoved(Item itemRemoved)
        {
            var itemPresenterToRemove = _itemPresentersSpawned.Find(itemPresenter => itemPresenter.ItemId == itemRemoved.GetInstanceID());
            _itemPresentersSpawned.Remove(itemPresenterToRemove);
        }
        
        private void OpenItem(InventoryItemPresenterUiToolkit inventoryItemPresenter)
        {
            if (_currentInventoryItemPresenter != null)
            {
                if (_currentInventoryItemPresenter != inventoryItemPresenter)
                {
                    RemoveInventoryItemPage(_currentInventoryItemPresenter);
                }
                else
                {
                    return;
                }
            }

            _currentInventoryItemPresenter = inventoryItemPresenter;
            _inventoryItemPage.style.display = DisplayStyle.Flex;
           // inventoryItemPresenter.ItemData.InventoryPage.CloneTree(_inventoryItemPage.Q("Container"));
            _inventoryItemPageSpawned = _currentInventoryItemPresenter.ItemData.InventoryPage.CloneTree();
            _inventoryItemPageContainer.Add(_inventoryItemPageSpawned);
            _inventoryItemPageExitButton.RegisterCallback<ClickEvent>(_=> CloseItem(_currentInventoryItemPresenter));
            _currentInventoryItemPresenter.ItemData.onItemSelectedOnInventory.RaiseEvent();
            interactWithObject?.action.Disable();
        }

        private void CloseItem(InventoryItemPresenterUiToolkit inventoryItemPresenter)
        {
            _currentInventoryItemPresenter = null;
            RemoveInventoryItemPage(inventoryItemPresenter);
            _inventoryItemPage.style.display = DisplayStyle.None;
            interactWithObject?.action.Enable();
        }

        private void RemoveInventoryItemPage(InventoryItemPresenterUiToolkit inventoryItemPresenter)
        {
            _inventoryItemPageExitButton.UnregisterCallback<ClickEvent>(_=> CloseItem(inventoryItemPresenter));
            _inventoryItemPageSpawned.RemoveFromHierarchy();
            inventoryItemPresenter.ItemData.onItemClosedOnInventory.RaiseEvent();
        }
    }
}
