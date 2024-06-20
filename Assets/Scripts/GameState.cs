using System;
using DataModel.Base;
using DataModel.Data;
using DataModel.Interface;

public class GameState
{
    private EventStream _stream;
    private GameStateData _stateData;
    
    public GameState(GameStateData stateData, EventStream eventStream)
    {
        _stream = eventStream;
        _stateData = stateData;
    }

    public IGameStateData Data => _stateData;

    public void Update(Action<GameStateData> action)
    {
        _stream.Enqueue(action);
        _stream.Flush();
    }
}