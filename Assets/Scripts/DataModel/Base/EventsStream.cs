using System;
using System.Collections.Concurrent;

namespace DataModel.Base
{
    internal class EventsStream : IDisposable
    {
        private ConcurrentQueue<Action> _queue = new();
        private Action _current;

        public void Enqueue(Action action)
        {
            if (action != null)
            {
                _queue.Enqueue(action);
            }
        }

        void IDisposable.Dispose()
        {
            while (_queue.TryDequeue(out _current))
            {
                _current.Invoke();
            }
        }
    }
}
