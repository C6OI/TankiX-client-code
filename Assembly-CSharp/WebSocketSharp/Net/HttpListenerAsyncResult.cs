using System;
using System.Threading;

namespace WebSocketSharp.Net {
    internal class HttpListenerAsyncResult : IAsyncResult {
        readonly AsyncCallback _callback;

        bool _completed;

        HttpListenerContext _context;

        Exception _exception;

        readonly object _sync;

        ManualResetEvent _waitHandle;

        internal HttpListenerAsyncResult(AsyncCallback callback, object state) {
            _callback = callback;
            AsyncState = state;
            _sync = new object();
        }

        internal bool EndCalled { get; set; }

        internal bool InGet { get; set; }

        public object AsyncState { get; }

        public WaitHandle AsyncWaitHandle {
            get {
                lock (_sync) {
                    return _waitHandle ?? (_waitHandle = new ManualResetEvent(_completed));
                }
            }
        }

        public bool CompletedSynchronously { get; private set; }

        public bool IsCompleted {
            get {
                lock (_sync) {
                    return _completed;
                }
            }
        }

        static void complete(HttpListenerAsyncResult asyncResult) {
            lock (asyncResult._sync) {
                asyncResult._completed = true;
                ManualResetEvent waitHandle = asyncResult._waitHandle;

                if (waitHandle != null) {
                    waitHandle.Set();
                }
            }

            AsyncCallback callback = asyncResult._callback;

            if (callback == null) {
                return;
            }

            ThreadPool.QueueUserWorkItem(delegate {
                try {
                    callback(asyncResult);
                } catch { }
            }, null);
        }

        internal void Complete(Exception exception) {
            _exception = !InGet || !(exception is ObjectDisposedException) ? exception : new HttpListenerException(995, "The listener is closed.");
            complete(this);
        }

        internal void Complete(HttpListenerContext context) {
            Complete(context, false);
        }

        internal void Complete(HttpListenerContext context, bool syncCompleted) {
            _context = context;
            CompletedSynchronously = syncCompleted;
            complete(this);
        }

        internal HttpListenerContext GetContext() {
            if (_exception != null) {
                throw _exception;
            }

            return _context;
        }
    }
}