using DataModel.Data;

public class GameStateService
{
    private static readonly GameStateService _instance = new();

    public static GameStateService Get()
    {
        return _instance;
    }

    public GameState State { get; private set; }

    public void Init(GameStateData stateData)
    {
        State = new(stateData);
    }
}