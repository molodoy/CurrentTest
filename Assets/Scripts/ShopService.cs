using DataModel.Base;
using DataModel.Data;

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
        using (GameStateTransaction transaction = gameState.OpenModification())
        {
            transaction.Data.Stars += stars;
            transaction.Data.Coins -= forCoins;
        }
    }

    public void UseCoins(int coins)
    {
        GameState gameState = GameStateService.Get().State;
        using (GameStateTransaction transaction = gameState.OpenModification())
        {
            transaction.Data.Coins -= coins;
        }
    }

    public void BuyInventoryItem(InventoryItemData item, int forCoins)
    {
        GameState gameState = GameStateService.Get().State;
        using (GameStateTransaction transaction = gameState.OpenModification())
        {
            transaction.Data.Inventory.Items.Add(item);
            transaction.Data.Coins -= forCoins;
        }
    }
}