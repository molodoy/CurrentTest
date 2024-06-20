using System;
using DataModel.Data;

namespace DataModel.Base
{
    public class GameStateTransaction : IDisposable
    {
        private GameStateData _targetData;
        
        public GameStateData Data { get; }

        public GameStateTransaction(GameStateData transactionData, GameStateData targetData)
        {
            Data = transactionData;

            _targetData = targetData;
        }

        void IDisposable.Dispose()
        {
            lock (_targetData)
            {
                using (EventsStream eventsStream = new EventsStream())
                {
                    _targetData.Update(Data, eventsStream);   
                }
            }
        }
    }
}