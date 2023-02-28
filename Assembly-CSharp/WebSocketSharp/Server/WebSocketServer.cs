using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;
using AuthenticationSchemes = WebSocketSharp.Net.AuthenticationSchemes;
using HttpStatusCode = WebSocketSharp.Net.HttpStatusCode;
using NetworkCredential = WebSocketSharp.Net.NetworkCredential;

namespace WebSocketSharp.Server {
    public class WebSocketServer {
        static readonly string _defaultRealm;

        AuthenticationSchemes _authSchemes;

        bool _dnsStyle;

        string _hostname;

        TcpListener _listener;

        string _realm;

        Thread _receiveThread;

        bool _reuseAddress;

        ServerSslConfiguration _sslConfig;

        volatile ServerState _state;

        object _sync;

        Func<IIdentity, NetworkCredential> _userCredFinder;

        static WebSocketServer() => _defaultRealm = "SECRET AREA";

        public WebSocketServer() {
            init(null, IPAddress.Any, 80, false);
        }

        public WebSocketServer(int port)
            : this(port, port == 443) { }

        public WebSocketServer(string url) {
            if (url == null) {
                throw new ArgumentNullException("url");
            }

            if (url.Length == 0) {
                throw new ArgumentException("An empty string.", "url");
            }

            Uri result;
            string message;

            if (!tryCreateUri(url, out result, out message)) {
                throw new ArgumentException(message, "url");
            }

            string dnsSafeHost = result.DnsSafeHost;
            IPAddress address = dnsSafeHost.ToIPAddress();

            if (!address.IsLocal()) {
                throw new ArgumentException("The host part isn't a local host name: " + url, "url");
            }

            init(dnsSafeHost, address, result.Port, result.Scheme == "wss");
        }

        public WebSocketServer(int port, bool secure) {
            if (!port.IsPortNumber()) {
                throw new ArgumentOutOfRangeException("port", "Not between 1 and 65535 inclusive: " + port);
            }

            init(null, IPAddress.Any, port, secure);
        }

        public WebSocketServer(IPAddress address, int port)
            : this(address, port, port == 443) { }

        public WebSocketServer(IPAddress address, int port, bool secure) {
            if (address == null) {
                throw new ArgumentNullException("address");
            }

            if (!address.IsLocal()) {
                throw new ArgumentException("Not a local IP address: " + address, "address");
            }

            if (!port.IsPortNumber()) {
                throw new ArgumentOutOfRangeException("port", "Not between 1 and 65535 inclusive: " + port);
            }

            init(null, address, port, secure);
        }

        public IPAddress Address { get; private set; }

        public AuthenticationSchemes AuthenticationSchemes {
            get => _authSchemes;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _authSchemes = value;
                }
            }
        }

        public bool IsListening => _state == ServerState.Start;

        public bool IsSecure { get; private set; }

        public bool KeepClean {
            get => WebSocketServices.KeepClean;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    WebSocketServices.KeepClean = value;
                }
            }
        }

        public Logger Log { get; private set; }

        public int Port { get; private set; }

        public string Realm {
            get => _realm;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _realm = value;
                }
            }
        }

        public bool ReuseAddress {
            get => _reuseAddress;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _reuseAddress = value;
                }
            }
        }

        public ServerSslConfiguration SslConfiguration {
            get => _sslConfig ?? (_sslConfig = new ServerSslConfiguration(null));
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _sslConfig = value;
                }
            }
        }

        public Func<IIdentity, NetworkCredential> UserCredentialsFinder {
            get => _userCredFinder;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _userCredFinder = value;
                }
            }
        }

        public TimeSpan WaitTime {
            get => WebSocketServices.WaitTime;
            set {
                string text = _state.CheckIfAvailable(true, false, false) ?? value.CheckIfValidWaitTime();

                if (text != null) {
                    Log.Error(text);
                } else {
                    WebSocketServices.WaitTime = value;
                }
            }
        }

        public WebSocketServiceManager WebSocketServices { get; private set; }

        void abort() {
            lock (_sync) {
                if (!IsListening) {
                    return;
                }

                _state = ServerState.ShuttingDown;
            }

            _listener.Stop();
            WebSocketServices.Stop(new CloseEventArgs(CloseStatusCode.ServerError), true, false);
            _state = ServerState.Stop;
        }

        string checkIfCertificateExists() => !IsSecure || _sslConfig != null && _sslConfig.ServerCertificate != null ? null : "The secure connection requires a server certificate.";

        string getRealm() {
            string realm = _realm;
            return realm == null || realm.Length <= 0 ? _defaultRealm : realm;
        }

        void init(string hostname, IPAddress address, int port, bool secure) {
            _hostname = hostname ?? address.ToString();
            Address = address;
            Port = port;
            IsSecure = secure;
            _authSchemes = AuthenticationSchemes.Anonymous;
            _dnsStyle = Uri.CheckHostName(hostname) == UriHostNameType.Dns;
            _listener = new TcpListener(address, port);
            Log = new Logger();
            WebSocketServices = new WebSocketServiceManager(Log);
            _sync = new object();
        }

        void processRequest(TcpListenerWebSocketContext context) {
            Uri requestUri = context.RequestUri;

            if (requestUri == null || requestUri.Port != Port) {
                context.Close(HttpStatusCode.BadRequest);
                return;
            }

            if (_dnsStyle) {
                string dnsSafeHost = requestUri.DnsSafeHost;

                if (Uri.CheckHostName(dnsSafeHost) == UriHostNameType.Dns && dnsSafeHost != _hostname) {
                    context.Close(HttpStatusCode.NotFound);
                    return;
                }
            }

            WebSocketServiceHost host;

            if (!WebSocketServices.InternalTryGetServiceHost(requestUri.AbsolutePath, out host)) {
                context.Close(HttpStatusCode.NotImplemented);
            } else {
                host.StartSession(context);
            }
        }

        void receiveRequest() {
            while (true) {
                try {
                    TcpClient cl = _listener.AcceptTcpClient();

                    ThreadPool.QueueUserWorkItem(delegate {
                        try {
                            TcpListenerWebSocketContext webSocketContext = cl.GetWebSocketContext(null, IsSecure, _sslConfig, Log);

                            if (webSocketContext.Authenticate(_authSchemes, getRealm(), _userCredFinder)) {
                                processRequest(webSocketContext);
                            }
                        } catch (Exception ex3) {
                            Log.Fatal(ex3.ToString());
                            cl.Close();
                        }
                    });
                } catch (SocketException ex) {
                    Log.Warn("Receiving has been stopped.\n  reason: " + ex.Message);
                    break;
                } catch (Exception ex2) {
                    Log.Fatal(ex2.ToString());
                    break;
                }
            }

            if (IsListening) {
                abort();
            }
        }

        void startReceiving() {
            if (_reuseAddress) {
                _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            }

            _listener.Start();
            _receiveThread = new Thread(receiveRequest);
            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }

        void stopReceiving(int millisecondsTimeout) {
            _listener.Stop();
            _receiveThread.Join(millisecondsTimeout);
        }

        static bool tryCreateUri(string uriString, out Uri result, out string message) {
            if (!uriString.TryCreateWebSocketUri(out result, out message)) {
                return false;
            }

            if (result.PathAndQuery != "/") {
                result = null;
                message = "Includes the path or query component: " + uriString;
                return false;
            }

            return true;
        }

        public void AddWebSocketService<TBehavior>(string path, Func<TBehavior> initializer) where TBehavior : WebSocketBehavior {
            string text = path.CheckIfValidServicePath() ?? (initializer != null ? null : "'initializer' is null.");

            if (text != null) {
                Log.Error(text);
            } else {
                WebSocketServices.Add(path, initializer);
            }
        }

        public void AddWebSocketService<TBehaviorWithNew>(string path) where TBehaviorWithNew : WebSocketBehavior, new() {
            AddWebSocketService(path, () => new TBehaviorWithNew());
        }

        public bool RemoveWebSocketService(string path) {
            string text = path.CheckIfValidServicePath();

            if (text != null) {
                Log.Error(text);
                return false;
            }

            return WebSocketServices.Remove(path);
        }

        public void Start() {
            lock (_sync) {
                string text = _state.CheckIfAvailable(true, false, false) ?? checkIfCertificateExists();

                if (text != null) {
                    Log.Error(text);
                    return;
                }

                WebSocketServices.Start();
                startReceiving();
                _state = ServerState.Start;
            }
        }

        public void Stop() {
            lock (_sync) {
                string text = _state.CheckIfAvailable(false, true, false);

                if (text != null) {
                    Log.Error(text);
                    return;
                }

                _state = ServerState.ShuttingDown;
            }

            stopReceiving(5000);
            WebSocketServices.Stop(new CloseEventArgs(), true, true);
            _state = ServerState.Stop;
        }

        public void Stop(ushort code, string reason) {
            lock (_sync) {
                string text = _state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckCloseParameters(code, reason, false);

                if (text != null) {
                    Log.Error(text);
                    return;
                }

                _state = ServerState.ShuttingDown;
            }

            stopReceiving(5000);

            if (code == 1005) {
                WebSocketServices.Stop(new CloseEventArgs(), true, true);
            } else {
                bool flag = !code.IsReserved();
                WebSocketServices.Stop(new CloseEventArgs(code, reason), flag, flag);
            }

            _state = ServerState.Stop;
        }

        public void Stop(CloseStatusCode code, string reason) {
            lock (_sync) {
                string text = _state.CheckIfAvailable(false, true, false) ?? WebSocket.CheckCloseParameters(code, reason, false);

                if (text != null) {
                    Log.Error(text);
                    return;
                }

                _state = ServerState.ShuttingDown;
            }

            stopReceiving(5000);

            if (code == CloseStatusCode.NoStatus) {
                WebSocketServices.Stop(new CloseEventArgs(), true, true);
            } else {
                bool flag = !code.IsReserved();
                WebSocketServices.Stop(new CloseEventArgs(code, reason), flag, flag);
            }

            _state = ServerState.Stop;
        }
    }
}