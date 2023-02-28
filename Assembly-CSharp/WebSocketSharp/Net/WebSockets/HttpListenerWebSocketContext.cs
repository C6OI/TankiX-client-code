using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Principal;

namespace WebSocketSharp.Net.WebSockets {
    public class HttpListenerWebSocketContext : WebSocketContext {
        readonly HttpListenerContext _context;

        internal HttpListenerWebSocketContext(HttpListenerContext context, string protocol) {
            _context = context;
            WebSocket = new WebSocket(this, protocol);
        }

        internal Logger Log => _context.Listener.Log;

        internal Stream Stream => _context.Connection.Stream;

        public override CookieCollection CookieCollection => _context.Request.Cookies;

        public override NameValueCollection Headers => _context.Request.Headers;

        public override string Host => _context.Request.Headers["Host"];

        public override bool IsAuthenticated => _context.User != null;

        public override bool IsLocal => _context.Request.IsLocal;

        public override bool IsSecureConnection => _context.Connection.IsSecure;

        public override bool IsWebSocketRequest => _context.Request.IsWebSocketRequest;

        public override string Origin => _context.Request.Headers["Origin"];

        public override NameValueCollection QueryString => _context.Request.QueryString;

        public override Uri RequestUri => _context.Request.Url;

        public override string SecWebSocketKey => _context.Request.Headers["Sec-WebSocket-Key"];

        public override IEnumerable<string> SecWebSocketProtocols {
            get {
                string protocols = _context.Request.Headers["Sec-WebSocket-Protocol"];

                if (protocols != null) {
                    string[] array = protocols.Split(',');

                    foreach (string protocol in array) {
                        yield return protocol.Trim();
                    }
                }
            }
        }

        public override string SecWebSocketVersion => _context.Request.Headers["Sec-WebSocket-Version"];

        public override IPEndPoint ServerEndPoint => _context.Connection.LocalEndPoint;

        public override IPrincipal User => _context.User;

        public override IPEndPoint UserEndPoint => _context.Connection.RemoteEndPoint;

        public override WebSocket WebSocket { get; }

        internal void Close() {
            _context.Connection.Close(true);
        }

        internal void Close(HttpStatusCode code) {
            _context.Response.Close(code);
        }

        public override string ToString() => _context.Request.ToString();
    }
}