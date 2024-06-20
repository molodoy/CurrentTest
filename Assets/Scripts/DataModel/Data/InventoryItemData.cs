using System;
using DataModel.Base;
using DataModel.Interface;

namespace DataModel.Data
{
    public class InventoryItemData : IInventoryItemData
    {
        private EventStream _eventStream;
        private int _id;
        private int _amount;
        private string _localizationId;

        private event Action _amountChanged;
        
        #region IInventoryItemData

        int IInventoryItemData.Id => _id;

        int IInventoryItemData.Amount => _amount;

        event Action IInventoryItemData.AmountChanged
        {
            add => _amountChanged += value;
            remove => _amountChanged -= value;
        }

        string IInventoryItemData.LocalizationId => _localizationId;
        
        #endregion

        #region Public

        public InventoryItemData(int id, string localizationId, EventStream eventStream)
        {
            _eventStream = eventStream;
            _id = id;
            _localizationId = localizationId;
        }

        public int Id => _id;

        public int Amount
        {
            get => _amount;
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    
                    _eventStream.Enqueue(_ =>
                    {
                        _amountChanged?.Invoke();
                    });
                }
            }
        }

        public string LocalizationId => _localizationId;

        #endregion
    }
}