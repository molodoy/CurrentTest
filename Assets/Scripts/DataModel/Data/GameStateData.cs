using System;
using DataModel.Base;
using DataModel.Interface;

namespace DataModel.Data
{
    public class GameStateData : IGameStateData
    {
        private EventStream _eventStream;
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

        #endregion

        #region Public

        public GameStateData(EventStream eventStream)
        {
            _eventStream = eventStream;
            _coins = 0;
            _stars = 0;
            _inventoryData = new InventoryData(_eventStream);
        }

        public int Coins
        {
            get => _coins;
            set
            {
                if (_coins != value)
                {
                    _coins = value;

                    _eventStream.Enqueue(_ =>
                    {
                        _coinsChanged?.Invoke();
                    });
                }
            }
        }

        public int Stars
        {
            get => _stars;
            set
            {
                if (_stars != value)
                {
                    _stars = value;
                    
                    _eventStream.Enqueue(_ =>
                    {
                        _starsChanged?.Invoke();
                    });
                }
            }
        }

        public InventoryData Inventory => _inventoryData;

        #endregion
    }
}