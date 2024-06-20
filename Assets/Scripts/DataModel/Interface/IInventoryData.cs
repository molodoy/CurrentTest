using System;
using System.Collections.Generic;

namespace DataModel.Interface
{
    public interface IInventoryData
    {
        public IReadOnlyList<IInventoryItemData> Items { get; }
        
        public event Action ItemsChanged;
    }
}
