using DataModel.Data;
using DataModel.Interface;
using NUnit.Framework;

namespace Tests
{
    public class TestSuite
    {
        [Test]
        public void CanInitGameState()
        {
            GameStateService gameStateService = GameStateService.Get();
            gameStateService.Init(new GameStateData
            {
                Coins = 10,
                Stars = 0
            });

            GameStateData stateDataCopy = gameStateService.State.GetCopyThreadSafe();
            Assert.That(stateDataCopy.Coins, Is.EqualTo(10));
            Assert.That(stateDataCopy.Stars, Is.EqualTo(0));
        }

        [Test]
        public void CanObserveGameStateChanges()
        {
            GameStateService gameStateService = GameStateService.Get();
            gameStateService.Init(new GameStateData
            {
                Coins = 10,
                Stars = 0
            });

            int stateObserverCalled = 0;
            void StateValidator()
            {
                ++stateObserverCalled;

                GameStateData stateDataCopy = gameStateService.State.GetCopyThreadSafe();
                Assert.That(stateDataCopy.Coins, Is.EqualTo(8));
            };
            
            IGameStateData stateData = gameStateService.State.Data;
            stateData.CoinsChanged += StateValidator;
            
            ShopService.Get().UseCoins(2);

            Assert.That(stateObserverCalled, Is.EqualTo(1));
        }
    
        [Test]
        public void CanObserveConsistentGameStateChanges()
        {
            GameStateService gameStateService = GameStateService.Get();
            gameStateService.Init(new GameStateData
            {
                Coins = 10,
                Stars = 0
            });

            int stateObserverCalled = 0;
            void StateValidator()
            {
                ++stateObserverCalled;

                GameStateData stateDataCopy = gameStateService.State.GetCopyThreadSafe();
                Assert.That(stateDataCopy.Stars, Is.EqualTo(1));
                Assert.That(stateDataCopy.Coins, Is.EqualTo(9));
            }

            IGameStateData stateData = gameStateService.State.Data;
            stateData.CoinsChanged += StateValidator;
            stateData.StarsChanged += StateValidator;

            var shopService = ShopService.Get();
            shopService.BuyStars(1, 1);
            
            Assert.That(stateObserverCalled, Is.EqualTo(2));
        }
        
        [Test]
        public void CanObserveInventoryChanges()
        {
            var gameStateService = GameStateService.Get();
            gameStateService.Init(new GameStateData
            {
                Coins = 10,
                Stars = 0
            });

            int stateObserverCalled = 0;
            void StateValidator()
            {
                ++stateObserverCalled;

                GameStateData stateDataCopy = gameStateService.State.GetCopyThreadSafe();
                Assert.That(stateDataCopy.Stars, Is.EqualTo(0));
                Assert.That(stateDataCopy.Coins, Is.EqualTo(5));
                Assert.That(stateDataCopy.Inventory.Items.Count, Is.EqualTo(1));
            }

            IGameStateData stateData = gameStateService.State.Data;
            stateData.CoinsChanged += StateValidator;
            stateData.StarsChanged += StateValidator;
            stateData.Inventory.ItemsChanged += StateValidator;

            var shopService = ShopService.Get();
            InventoryItemData item = new InventoryItemData(0, "Booster")
            {
                Amount = 1
            };
            
            shopService.BuyInventoryItem(item, 5);
            
            Assert.That(stateObserverCalled, Is.EqualTo(2));
        }
    }
}