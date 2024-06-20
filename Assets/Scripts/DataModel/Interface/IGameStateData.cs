using System;

namespace DataModel.Interface
{
    public interface IGameStateData
    {
        public int Coins { get; }
        
        public event Action CoinsChanged;

        public int Stars { get; }

        public event Action StarsChanged;
        
        public IInventoryData Inventory { get;  }

        public event Action Updated;
    }
}