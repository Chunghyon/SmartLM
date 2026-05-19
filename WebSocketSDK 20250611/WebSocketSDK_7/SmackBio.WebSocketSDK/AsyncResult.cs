using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SmackBio.WebSocketSDK
{
    class AsyncResult : IAsyncResult
    {
        bool _disposed = false;
        object _monitor = new object();
        ManualResetEvent _completionEvent = null;
        AsyncCallback _callback;
        object _extraData;
        bool _completedSynchronously = false;
        bool _completed = false;
        Exception _exception = null;

        internal object monitor { get { return _monitor; } }

        public object AsyncState { get { return _extraData; } }
        public WaitHandle AsyncWaitHandle { get {
            lock (_monitor)
            {
                if (_disposed)
                    return null;

                if (_completionEvent == null)
                    _completionEvent = new ManualResetEvent(_completed);
                return _completionEvent;
            }
        } }
        public bool CompletedSynchronously { get {
            lock (_monitor) return _completedSynchronously;
        } }
        public bool IsCompleted { get {
            lock (_monitor) return _completed;
        } }

        public AsyncResult(AsyncCallback cb, object extraData)
        {
            _callback = cb;
            _extraData = extraData;
        }

        public void setException(Exception ex)
        {
            _exception = ex;
        }

        virtual public void complete(bool isSynchronous)
        {
            lock (_monitor)
            {
                _completedSynchronously = isSynchronous;
                _completed = true;
                if (_completionEvent != null)
                    _completionEvent.Set();
                Monitor.PulseAll(_monitor);
            }

            if (_callback != null)
                _callback(this);
        }

        public void endOperation()
        {
            lock (_monitor)
            {
                while (!_completed)
                    Monitor.Wait(_monitor);

                _disposed = true;
                if (_completionEvent != null)
                {
                    _completionEvent.Dispose();
                    _completionEvent = null;
                }
            }

            if (_exception != null)
                throw _exception;
        }
    }

    class AsyncResultWithValue<T> : AsyncResult
    {
        T _value;

        public AsyncResultWithValue(AsyncCallback cb, object extraData)
            : base(cb, extraData)
        { }

        public void setValue(T value)
        {
            _value = value;
        }

        public new T endOperation()
        {
            base.endOperation();
            return _value;
        }
    }
}
