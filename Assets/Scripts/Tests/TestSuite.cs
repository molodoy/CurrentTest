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
            gameStateService.Init(10, 0);

            IGameStateData stateData = gameStateService.State.Data;
            Assert.That(stateData.Coins, Is.EqualTo(10));
            Assert.That(stateData.Stars, Is.EqualTo(0));
        }

        [Test]
        public void CanObserveGameStateChanges()
        {
            GameStateService gameStateService = GameStateService.Get();
            gameStateService.Init(10, 0);

            int stateObserverCalled = 0;
            void StateValidator()
            {
                ++stateObserverCalled;

                IGameStateData stateData = gameStateService.State.Data;
                Assert.That(stateData.Coins, Is.EqualTo(8));
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
            gameStateService.Init(10, 0);

            int stateObserverCalled = 0;
            void StateValidator()
            {
                ++stateObserverCalled;

                IGameStateData stateData = gameStateService.State.Data;
                Assert.That(stateData.Stars, Is.EqualTo(1));
                Assert.That(stateData.Coins, Is.EqualTo(9));
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
            gameStateService.Init(10, 0);

            int stateObserverCalled = 0;
            void StateValidator()
            {
                ++stateObserverCalled;

                IGameStateData stateData = gameStateService.State.Data;
                Assert.That(stateData.Stars, Is.EqualTo(0));
                Assert.That(stateData.Coins, Is.EqualTo(5));
                Assert.That(stateData.Inventory.Items.Count, Is.EqualTo(1));
            }

            IGameStateData stateData = gameStateService.State.Data;
            stateData.CoinsChanged += StateValidator;
            stateData.StarsChanged += StateValidator;
            stateData.Inventory.ItemsChanged += StateValidator;

            var shopService = ShopService.Get();
            shopService.BuyInventoryItem(0, "Booster", 5);
            
            Assert.That(stateObserverCalled, Is.EqualTo(2));
        }
    }
}