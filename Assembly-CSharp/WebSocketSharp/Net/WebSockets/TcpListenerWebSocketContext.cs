using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;

namespace WebSocketSharp.Net.WebSockets {
    internal class TcpListenerWebSocketContext : WebSocketContext {
        CookieCollection _cookies;

        NameValueCollection _queryString;

        HttpRequest _request;

        readonly TcpClient _tcpClient;

        readonly Uri _uri;

        IPrincipal _user;

        readonly WebSocket _websocket;

        internal TcpListenerWebSocketContext(TcpClient tcpClient, string protocol, bool secure, ServerSslConfiguration sslConfig, Logger logger) {
            _tcpClient = tcpClient;
            IsSecureConnection = secure;
            Log = logger;
            NetworkStream stream = tcpClient.GetStream();

            if (secure) {
                SslStream sslStream = new(stream, false, sslConfig.ClientCertificateValidationCallback);
                sslStream.AuthenticateAsServer(sslConfig.ServerCertificate, sslConfig.ClientCertificateRequired, sslConfig.EnabledSslProtocols, sslConfig.CheckCertificateRevocation);
                Stream = sslStream;
            } else {
                Stream = stream;
            }

            _request = HttpRequest.Read(Stream, 90000);
            _uri = HttpUtility.CreateRequestUrl(_request.RequestUri, _request.Headers["Host"], _request.IsWebSocketRequest, secure);
            _websocket = new WebSocket(this, protocol);
        }

        internal Logger Log { get; }

        internal Stream Stream { get; }

        public override CookieCollection CookieCollection => _cookies ?? (_cookies = _request.Cookies);

        public override NameValueCollection Headers => _request.Headers;

        public override string Host => _request.Headers["Host"];

        public override bool IsAuthenticated => _user != null;

        public override bool IsLocal => UserEndPoint.Address.IsLocal();

        public override bool IsSecureConnection { get; }

        public override bool IsWebSocketRequest => _request.IsWebSocketRequest;

        public override string Origin => _request.Headers["Origin"];

        public override NameValueCollection QueryString => _queryString ?? (_queryString = HttpUtility.InternalParseQueryString(!(_uri != null) ? null : _uri.Query, Encoding.UTF8));

        public override Uri RequestUri => _uri;

        public override string SecWebSocketKey => _request.Headers["Sec-WebSocket-Key"];

        public override IEnumerable<string> SecWebSocketProtocols {
            get {
                string protocols = _request.Headers["Sec-WebSocket-Protocol"];

                if (protocols != null) {
                    string[] array = protocols.Split(',');

                    foreach (string protocol in array) {
                        yield return protocol.Trim();
                    }
                }
            }
        }

        public override string SecWebSocketVersion => _request.Headers["Sec-WebSocket-Version"];

        public override IPEndPoint ServerEndPoint => (IPEndPoint)_tcpClient.Client.LocalEndPoint;

        public override IPrincipal User => _user;

        public override IPEndPoint UserEndPoint => (IPEndPoint)_tcpClient.Client.RemoteEndPoint;

        public override WebSocket WebSocket => _websocket;

        internal bool Authenticate(AuthenticationSchemes scheme, string realm, Func<IIdentity, NetworkCredential> credentialsFinder) {
            if (scheme == AuthenticationSchemes.Anonymous) {
                return true;
            }

            if (scheme == AuthenticationSchemes.None) {
                Close(HttpStatusCode.Forbidden);
                return false;
            }

            string chal = new AuthenticationChallenge(scheme, realm).ToString();
            int retry = -1;
            Func<bool> auth = null;

            auth = delegate {
                retry++;

                if (retry > 99) {
                    Close(HttpStatusCode.Forbidden);
                    return false;
                }

                IPrincipal principal = HttpUtility.CreateUser(_request.Headers["Authorization"], scheme, realm, _request.HttpMethod, credentialsFinder);

                if (principal == null || !principal.Identity.IsAuthenticated) {
                    SendAuthenticationChallenge(chal);
                    return auth();
                }

                _user = principal;
                return true;
            };

            return auth();
        }

        internal void Close() {
            Stream.Close();
            _tcpClient.Close();
        }

        internal void Close(HttpStatusCode code) {
            _websocket.Close(HttpResponse.CreateCloseResponse(code));
        }

        internal void SendAuthenticationChallenge(string challenge) {
            byte[] array = HttpResponse.CreateUnauthorizedResponse(challenge).ToByteArray();
            Stream.Write(array, 0, array.Length);
            _request = HttpRequest.Read(Stream, 15000);
        }

        public override string ToString() => _request.ToString();
    }
}