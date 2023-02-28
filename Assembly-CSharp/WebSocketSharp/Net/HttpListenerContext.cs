using System;
using System.Security.Principal;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp.Net {
    public sealed class HttpListenerContext {
        HttpListenerWebSocketContext _websocketContext;

        internal HttpListenerContext(HttpConnection connection) {
            Connection = connection;
            ErrorStatus = 400;
            Request = new HttpListenerRequest(this);
            Response = new HttpListenerResponse(this);
        }

        internal HttpConnection Connection { get; }

        internal string ErrorMessage { get; set; }

        internal int ErrorStatus { get; set; }

        internal bool HasError => ErrorMessage != null;

        internal HttpListener Listener { get; set; }

        public HttpListenerRequest Request { get; }

        public HttpListenerResponse Response { get; }

        public IPrincipal User { get; private set; }

        internal bool Authenticate() {
            AuthenticationSchemes authenticationSchemes = Listener.SelectAuthenticationScheme(Request);

            switch (authenticationSchemes) {
                case AuthenticationSchemes.Anonymous:
                    return true;

                case AuthenticationSchemes.None:
                    Response.Close(HttpStatusCode.Forbidden);
                    return false;

                default: {
                    string realm = Listener.GetRealm();
                    IPrincipal principal = HttpUtility.CreateUser(Request.Headers["Authorization"], authenticationSchemes, realm, Request.HttpMethod, Listener.GetUserCredentialsFinder());

                    if (principal == null || !principal.Identity.IsAuthenticated) {
                        Response.CloseWithAuthChallenge(new AuthenticationChallenge(authenticationSchemes, realm).ToString());
                        return false;
                    }

                    User = principal;
                    return true;
                }
            }
        }

        public HttpListenerWebSocketContext AcceptWebSocket(string protocol) {
            if (_websocketContext != null) {
                throw new InvalidOperationException("The accepting is already in progress.");
            }

            if (protocol != null) {
                if (protocol.Length == 0) {
                    throw new ArgumentException("An empty string.", "protocol");
                }

                if (!protocol.IsToken()) {
                    throw new ArgumentException("Contains an invalid character.", "protocol");
                }
            }

            _websocketContext = new HttpListenerWebSocketContext(this, protocol);
            return _websocketContext;
        }
    }
}