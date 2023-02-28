using System;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server {
    public abstract class WebSocketServiceHost {
        internal ServerState State => Sessions.State;

        public abstract bool KeepClean { get; set; }

        public abstract string Path { get; }

        public abstract WebSocketSessionManager Sessions { get; }

        public abstract Type Type { get; }

        public abstract TimeSpan WaitTime { get; set; }

        internal void Start() {
            Sessions.Start();
        }

        internal void StartSession(WebSocketContext context) {
            CreateSession().Start(context, Sessions);
        }

        internal void Stop(ushort code, string reason) {
            CloseEventArgs closeEventArgs = new(code, reason);
            bool flag = !code.IsReserved();
            byte[] frameAsBytes = !flag ? null : WebSocketFrame.CreateCloseFrame(closeEventArgs.PayloadData, false).ToArray();
            Sessions.Stop(closeEventArgs, frameAsBytes, flag);
        }

        protected abstract WebSocketBehavior CreateSession();
    }

    internal class WebSocketServiceHost<TBehavior> : WebSocketServiceHost where TBehavior : WebSocketBehavior {
        readonly Func<TBehavior> _initializer;

        readonly Logger _logger;

        readonly WebSocketSessionManager _sessions;

        internal WebSocketServiceHost(string path, Func<TBehavior> initializer, Logger logger) {
            Path = path;
            _initializer = initializer;
            _logger = logger;
            _sessions = new WebSocketSessionManager(logger);
        }

        public override bool KeepClean {
            get => _sessions.KeepClean;
            set {
                string text = _sessions.State.CheckIfAvailable(true, false, false);

                if (text != null) {
                    _logger.Error(text);
                } else {
                    _sessions.KeepClean = value;
                }
            }
        }

        public override string Path { get; }

        public override WebSocketSessionManager Sessions => _sessions;

        public override Type Type => typeof(TBehavior);

        public override TimeSpan WaitTime {
            get => _sessions.WaitTime;
            set {
                string text = _sessions.State.CheckIfAvailable(true, false, false) ?? value.CheckIfValidWaitTime();

                if (text != null) {
                    _logger.Error(text);
                } else {
                    _sessions.WaitTime = value;
                }
            }
        }

        protected override WebSocketBehavior CreateSession() => _initializer();
    }
}