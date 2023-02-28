using System;
using System.IO;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Server {
    public abstract class WebSocketBehavior : IWebSocketSession {
        bool _emitOnPing;

        string _protocol;

        WebSocket _websocket;

        protected WebSocketBehavior() => StartTime = DateTime.MaxValue;

        protected Logger Log => _websocket == null ? null : _websocket.Log;

        protected WebSocketSessionManager Sessions { get; private set; }

        public Func<CookieCollection, CookieCollection, bool> CookiesValidator { get; set; }

        public bool EmitOnPing {
            get => _websocket == null ? _emitOnPing : _websocket.EmitOnPing;
            set {
                if (_websocket != null) {
                    _websocket.EmitOnPing = value;
                } else {
                    _emitOnPing = value;
                }
            }
        }

        public bool IgnoreExtensions { get; set; }

        public Func<string, bool> OriginValidator { get; set; }

        public WebSocketContext Context { get; private set; }

        public string ID { get; private set; }

        public string Protocol {
            get => _websocket == null ? _protocol ?? string.Empty : _websocket.Protocol;
            set {
                if (State == WebSocketState.Connecting && (value == null || value.Length != 0 && value.IsToken())) {
                    _protocol = value;
                }
            }
        }

        public DateTime StartTime { get; private set; }

        public WebSocketState State => _websocket != null ? _websocket.ReadyState : WebSocketState.Connecting;

        string checkHandshakeRequest(WebSocketContext context) => OriginValidator != null && !OriginValidator(context.Origin) ? "Includes no Origin header, or it has an invalid value." :
                                                                  CookiesValidator == null || CookiesValidator(context.CookieCollection, context.WebSocket.CookieCollection) ? null :
                                                                  "Includes no cookie, or an invalid cookie exists.";

        void onClose(object sender, CloseEventArgs e) {
            if (ID != null) {
                Sessions.Remove(ID);
                OnClose(e);
            }
        }

        void onError(object sender, ErrorEventArgs e) {
            OnError(e);
        }

        void onMessage(object sender, MessageEventArgs e) {
            OnMessage(e);
        }

        void onOpen(object sender, EventArgs e) {
            ID = Sessions.Add(this);

            if (ID == null) {
                _websocket.Close(CloseStatusCode.Away);
                return;
            }

            StartTime = DateTime.Now;
            OnOpen();
        }

        internal void Start(WebSocketContext context, WebSocketSessionManager sessions) {
            if (_websocket != null) {
                _websocket.Log.Error("This session has already been started.");
                context.WebSocket.Close(HttpStatusCode.ServiceUnavailable);
                return;
            }

            Context = context;
            Sessions = sessions;
            _websocket = context.WebSocket;
            _websocket.CustomHandshakeRequestChecker = checkHandshakeRequest;
            _websocket.EmitOnPing = _emitOnPing;
            _websocket.IgnoreExtensions = IgnoreExtensions;
            _websocket.Protocol = _protocol;
            TimeSpan waitTime = sessions.WaitTime;

            if (waitTime != _websocket.WaitTime) {
                _websocket.WaitTime = waitTime;
            }

            _websocket.OnOpen += onOpen;
            _websocket.OnMessage += onMessage;
            _websocket.OnError += onError;
            _websocket.OnClose += onClose;
            _websocket.InternalAccept();
        }

        protected void Error(string message, Exception exception) {
            if (message != null && message.Length > 0) {
                OnError(new ErrorEventArgs(message, exception));
            }
        }

        protected virtual void OnClose(CloseEventArgs e) { }

        protected virtual void OnError(ErrorEventArgs e) { }

        protected virtual void OnMessage(MessageEventArgs e) { }

        protected virtual void OnOpen() { }

        protected void Send(byte[] data) {
            if (_websocket != null) {
                _websocket.Send(data);
            }
        }

        protected void Send(FileInfo file) {
            if (_websocket != null) {
                _websocket.Send(file);
            }
        }

        protected void Send(string data) {
            if (_websocket != null) {
                _websocket.Send(data);
            }
        }

        protected void SendAsync(byte[] data, Action<bool> completed) {
            if (_websocket != null) {
                _websocket.SendAsync(data, completed);
            }
        }

        protected void SendAsync(FileInfo file, Action<bool> completed) {
            if (_websocket != null) {
                _websocket.SendAsync(file, completed);
            }
        }

        protected void SendAsync(string data, Action<bool> completed) {
            if (_websocket != null) {
                _websocket.SendAsync(data, completed);
            }
        }

        protected void SendAsync(Stream stream, int length, Action<bool> completed) {
            if (_websocket != null) {
                _websocket.SendAsync(stream, length, completed);
            }
        }
    }
}