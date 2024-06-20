using System;
using System.Collections.Generic;
using DataModel.Base;
using DataModel.Interface;

namespace DataModel.Data
{
    public class InventoryData : IInventoryData
    {
        private List<InventoryItemData> _items;

        private event Action _itemsChanged;

        #region IInventoryData

        IReadOnlyList<IInventoryItemData> IInventoryData.Items => _items;

        event Action IInventoryData.ItemsChanged
        {
            add => _itemsChanged += value;
            remove => _itemsChanged -= value;
        }
        
        #endregion

        #region Public

        public InventoryData()
        {
            _items = new List<InventoryItemData>();
        }
        
        public InventoryData(IInventoryData data)
        {
            _items = new List<InventoryItemData>(data.Items.Count);
            foreach (IInventoryItemData item in data.Items)
            {
                _items.Add(new InventoryItemData(item));
            }
        }
        
        public List<InventoryItemData> Items => _items;

        #endregion

        internal void Update(InventoryData other, EventsStream eventsStream)
        {
            if (_items.MergeSlow(other._items))
            {
                eventsStream.Enqueue(_itemsChanged);   
            }
            
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Update(other._items[i], eventsStream);
            }
        }
    }

    internal static class Extension
    {
        private static int FindIndex(this List<InventoryItemData> items, InventoryItemData item)
        {
            return items.FindIndex(i => i.Id == item.Id);
        }

        internal static bool MergeSlow(this List<InventoryItemData> listFrom, List<InventoryItemData> listWith)
        {
            int removedCount = listFrom.RemoveAll(itemFrom => listWith.FindIndex(itemFrom) < 0);

            bool result = removedCount > 0;
            
            foreach (InventoryItemData itemWith in listWith)
            {
                int inventoryIndex = listFrom.FindIndex(itemWith);
                if (inventoryIndex < 0)
                {
                    result = true;

                    listFrom.Add(new InventoryItemData(itemWith));
                }
            }

            bool isOrderCorrect = true;
            for (int i = 0; i < listWith.Count; i++)
            {
                if (listFrom[i].Id != listWith[i].Id)
                {
                    isOrderCorrect = false;
                    break;
                }
            }

            if (!isOrderCorrect)
            {
                result = true;
                
                listFrom.Sort((a, b) => listWith.FindIndex(a).CompareTo(listWith.FindIndex(b)));
            }

            return result;
        }
    }
}
