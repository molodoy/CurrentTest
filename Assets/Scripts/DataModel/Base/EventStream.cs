using System;
using System.Collections.Concurrent;
using DataModel.Data;

namespace DataModel.Base
{
    public class EventStream
    {
        private GameStateData _stateData;
        private ConcurrentQueue<Action<GameStateData>> _queue = new();
        private bool _isFlushing;

        public void Init(GameStateData data)
        {
            _stateData = data;
        }
        
        public void Enqueue(Action<GameStateData> action)
        {
            if (_stateData != null)
            {
                _queue.Enqueue(action);
            }
        }

        public void Flush()
        {
            if (!_isFlushing)
            {
                _isFlushing = true;
                
                while (_queue.TryDequeue(out Action<GameStateData> current))
                {
                    current.Invoke(_stateData);
                }

                _isFlushing = false;
            }
        }
    }
}
