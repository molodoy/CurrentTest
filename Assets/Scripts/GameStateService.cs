using DataModel.Base;
using DataModel.Data;

public class GameStateService
{
    private static readonly GameStateService _instance = new();

    public static GameStateService Get()
    {
        return _instance;
    }

    public GameState State { get; private set; }

    public void Init(int coins, int stars)
    {
        EventStream eventStream = new EventStream();
        GameStateData gameStateData = new GameStateData(eventStream)
        {
            Coins = coins,
            Stars = stars
        };
        eventStream.Init(gameStateData);

        State = new GameState(gameStateData, eventStream);
    }
}