using System;
using System.Threading;

namespace WebSocketSharp.Net {
    internal class HttpStreamAsyncResult : IAsyncResult {
        readonly AsyncCallback _callback;

        bool _completed;

        readonly object _sync;

        ManualResetEvent _waitHandle;

        internal HttpStreamAsyncResult(AsyncCallback callback, object state) {
            _callback = callback;
            AsyncState = state;
            _sync = new object();
        }

        internal byte[] Buffer { get; set; }

        internal int Count { get; set; }

        internal Exception Exception { get; private set; }

        internal bool HasException => Exception != null;

        internal int Offset { get; set; }

        internal int SyncRead { get; set; }

        public object AsyncState { get; }

        public WaitHandle AsyncWaitHandle {
            get {
                lock (_sync) {
                    return _waitHandle ?? (_waitHandle = new ManualResetEvent(_completed));
                }
            }
        }

        public bool CompletedSynchronously => SyncRead == Count;

        public bool IsCompleted {
            get {
                lock (_sync) {
                    return _completed;
                }
            }
        }

        internal void Complete() {
            lock (_sync) {
                if (_completed) {
                    return;
                }

                _completed = true;

                if (_waitHandle != null) {
                    _waitHandle.Set();
                }

                if (_callback != null) {
                    _callback.BeginInvoke(this, delegate(IAsyncResult ar) {
                        _callback.EndInvoke(ar);
                    }, null);
                }
            }
        }

        internal void Complete(Exception exception) {
            Exception = exception;
            Complete();
        }
    }
}