using System;

namespace DataModel.Interface
{
    public interface IInventoryItemData
    {
        public int Id { get; }

        public int Amount { get; }
        
        public event Action AmountChanged;
    
        public string LocalizationId { get; }
    }
}