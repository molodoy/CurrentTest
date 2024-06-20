using System;
using DataModel.Base;
using DataModel.Interface;

namespace DataModel.Data
{
    public class InventoryItemData : IInventoryItemData
    {
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

        public InventoryItemData(int id, string localizationId)
        {
            _id = id;
            _localizationId = localizationId;
        }
        
        public InventoryItemData(IInventoryItemData data)
        {
            _id = data.Id;
            _amount = data.Amount;
            _localizationId = data.LocalizationId;
        }

        public int Id => _id;

        public int Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public string LocalizationId => _localizationId;

        #endregion

        internal void Update(InventoryItemData other, EventsStream eventsStream)
        {
            if (Amount != other.Amount)
            {
                Amount = other.Amount;
                eventsStream.Enqueue(_amountChanged);
            }
        }
    }
}