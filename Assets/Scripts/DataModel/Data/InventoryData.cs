using System;
using System.Collections.Generic;
using DataModel.Base;
using DataModel.Interface;

namespace DataModel.Data
{
    public class InventoryData : IInventoryData
    {
        private EventStream _eventStream;
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

        public InventoryData(EventStream stream)
        {
            _eventStream = stream;
            _items = new List<InventoryItemData>();
        }
        
        public IReadOnlyList<InventoryItemData> Items => _items;
        
        public void AddItem(int id, string localizationId, int amount)
        {
            InventoryItemData item = new InventoryItemData(id, localizationId, _eventStream)
            {
                Amount = amount
            };
            
            _items.Add(item);
            
            _eventStream.Enqueue(_ =>
            {
                _itemsChanged?.Invoke();
            });
        }
        
        public void RemoveItem(InventoryItemData itemData)
        {
            int index = FindItemIndex(itemData);
            if (index >= 0)
            {
                _items.RemoveAt(index);
                
                _eventStream.Enqueue(_ =>
                {
                    _itemsChanged?.Invoke();
                });
            }
        }

        public int FindItemIndex(int itemId)
        {
            return _items.FindIndex(item => item.Id == itemId);
        }
        
        public int FindItemIndex(InventoryItemData itemData)
        {
            return _items.FindIndex(item => item.Id == itemData.Id);
        }

        #endregion
    }
}
