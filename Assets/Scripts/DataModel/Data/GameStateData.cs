using System;
using DataModel.Base;
using DataModel.Interface;

namespace DataModel.Data
{
    public class GameStateData : IGameStateData
    {
        private int _coins;
        private int _stars;
        private InventoryData _inventoryData;

        private event Action _coinsChanged;
        private event Action _starsChanged;
        private event Action _updated;

        #region IGameStateData

        int IGameStateData.Coins => _coins;
        
        event Action IGameStateData.CoinsChanged
        {
            add => _coinsChanged += value;
            remove => _coinsChanged -= value;
        }
        
        int IGameStateData.Stars => _stars;
        
        event Action IGameStateData.StarsChanged
        {
            add => _starsChanged += value;
            remove => _starsChanged -= value;
        }

        IInventoryData IGameStateData.Inventory => _inventoryData;

        event Action IGameStateData.Updated
        {
            add => _updated += value;
            remove => _updated -= value;
        }

        #endregion

        #region Public

        public GameStateData()
        {
            _coins = 0;
            _stars = 0;
            _inventoryData = new InventoryData();
        }
        
        public GameStateData(IGameStateData data)
        {
            _coins = data.Coins;
            _stars = data.Stars;
            _inventoryData = new InventoryData(data.Inventory);
        }

        public int Coins
        {
            get => _coins;
            set => _coins = value;
        }

        public int Stars
        {
            get => _stars;
            set => _stars = value;
        }

        public InventoryData Inventory => _inventoryData;
        

        #endregion

        internal void Update(GameStateData other, EventsStream eventsStream)
        {
            if (_coins != other.Coins)
            {
                _coins = other.Coins;
                eventsStream.Enqueue(_coinsChanged);
            }

            if (_stars != other.Stars)
            {
                _stars = other.Stars;
                eventsStream.Enqueue(_starsChanged);
            }
        
            _inventoryData.Update(other.Inventory, eventsStream);
            
            eventsStream.Enqueue(_updated);
        }
    }
}