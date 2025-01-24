using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Game.DataEvent.ScriptableObject;
using UnityEngine;

namespace Game
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Instance;
        [SerializeField] private DataEventItem dataEventItemAdded;
        [SerializeField] private DataEventItem dataEventItemRemoved;
        ObservableCollection<Item> items = new ();
        [SerializeField] private Transform itemParentWhenFree;
        private void Awake()
        {
            Instance = this;
            items.CollectionChanged += ItemsOnCollectionChanged;
        }

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null && args.NewItems.Count != 0)
            {
                foreach (var item in args.NewItems)
                {
                    dataEventItemAdded.RaiseOnDataChanged((Item)item);
                }
            }

            if (args.OldItems != null && args.OldItems.Count != 0)
            {
                foreach (var item in args.OldItems)
                {
                    dataEventItemRemoved.RaiseOnDataChanged((Item)item);
                }
            }
        }

        public void AddItemToInventory(Item item)
        {
            items.Add(item);
            //item.transform.SetParent(transform);
        }

        public void RemoveItemFromInventory(Item item)
        {
            items.Remove(item);
            //item.transform.SetParent(itemParentWhenFree);
        }
    }
}
