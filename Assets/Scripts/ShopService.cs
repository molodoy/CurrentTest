public class ShopService
{
    private static readonly ShopService _instance = new();

    public static ShopService Get()
    {
        return _instance;
    }

    public void BuyStars(int stars, int forCoins)
    {
        GameState gameState = GameStateService.Get().State;
        gameState.Update(state =>
        {
            state.Stars += stars;
            state.Coins -= forCoins;
        });
    }

    public void UseCoins(int coins)
    {
        GameState gameState = GameStateService.Get().State;
        gameState.Update(state =>
        {
            state.Coins -= coins;
        });
    }

    public void BuyInventoryItem(int itemId, string localizationId, int forCoins)
    {
        GameState gameState = GameStateService.Get().State;
        gameState.Update(state =>
        {
            state.Inventory.AddItem(itemId, localizationId, 1);
            state.Coins -= forCoins;
        });
    }
}