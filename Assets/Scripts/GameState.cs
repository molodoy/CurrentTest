using DataModel.Base;
using DataModel.Data;
using DataModel.Interface;

public class GameState
{
    private GameStateData _stateData;
    
    public GameState(GameStateData stateData)
    {
        _stateData = stateData;
    }

    public IGameStateData Data => _stateData;
    
    public GameStateData GetCopyThreadSafe()
    {
        GameStateData result;
        lock (_stateData)
        {
            result = new GameStateData(_stateData);
        }

        return result;
    }

    public GameStateTransaction OpenModification()
    {
        GameStateData transactionData = GetCopyThreadSafe();
        GameStateTransaction result = new GameStateTransaction(transactionData, _stateData);
        return result;
    }
}