using System;
using System.IO;
using System.Net;
using System.Security.Principal;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;
using AuthenticationSchemes = WebSocketSharp.Net.AuthenticationSchemes;
using HttpListener = WebSocketSharp.Net.HttpListener;
using HttpListenerContext = WebSocketSharp.Net.HttpListenerContext;
using HttpListenerException = WebSocketSharp.Net.HttpListenerException;
using HttpStatusCode = WebSocketSharp.Net.HttpStatusCode;
using NetworkCredential = WebSocketSharp.Net.NetworkCredential;

namespace WebSocketSharp.Server {
    public class HttpServer {
        string _hostname;

        HttpListener _listener;

        Thread _receiveThread;

        string _rootPath;

        volatile ServerState _state;

        object _sync;

        bool _windows;

        public HttpServer() {
            init("*", IPAddress.Any, 80, false);
        }

        public HttpServer(int port)
            : this(port, port == 443) { }

        public HttpServer(string url) {
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

            init(dnsSafeHost, address, result.Port, result.Scheme == "https");
        }

        public HttpServer(int port, bool secure) {
            if (!port.IsPortNumber()) {
                throw new ArgumentOutOfRangeException("port", "Not between 1 and 65535 inclusive: " + port);
            }

            init("*", IPAddress.Any, port, secure);
        }

        public HttpServer(IPAddress address, int port)
            : this(address, port, port == 443) { }

        public HttpServer(IPAddress address, int port, bool secure) {
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
            get => _listener.AuthenticationSchemes;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _listener.AuthenticationSchemes = value;
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
            get => _listener.Realm;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _listener.Realm = value;
                }
            }
        }

        public bool ReuseAddress {
            get => _listener.ReuseAddress;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _listener.ReuseAddress = value;
                }
            }
        }

        public string RootPath {
            get => _rootPath == null || _rootPath.Length <= 0 ? _rootPath = "./Public" : _rootPath;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _rootPath = value;
                }
            }
        }

        public ServerSslConfiguration SslConfiguration {
            get => _listener.SslConfiguration;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _listener.SslConfiguration = value;
                }
            }
        }

        public Func<IIdentity, NetworkCredential> UserCredentialsFinder {
            get => _listener.UserCredentialsFinder;
            set {
                string text = _state.CheckIfAvailable(true, false, false);

                if (text != null) {
                    Log.Error(text);
                } else {
                    _listener.UserCredentialsFinder = value;
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

        public event EventHandler<HttpRequestEventArgs> OnConnect;

        public event EventHandler<HttpRequestEventArgs> OnDelete;

        public event EventHandler<HttpRequestEventArgs> OnGet;

        public event EventHandler<HttpRequestEventArgs> OnHead;

        public event EventHandler<HttpRequestEventArgs> OnOptions;

        public event EventHandler<HttpRequestEventArgs> OnPatch;

        public event EventHandler<HttpRequestEventArgs> OnPost;

        public event EventHandler<HttpRequestEventArgs> OnPut;

        public event EventHandler<HttpRequestEventArgs> OnTrace;

        void abort() {
            lock (_sync) {
                if (!IsListening) {
                    return;
                }

                _state = ServerState.ShuttingDown;
            }

            WebSocketServices.Stop(new CloseEventArgs(CloseStatusCode.ServerError), true, false);
            _listener.Abort();
            _state = ServerState.Stop;
        }

        string checkIfCertificateExists() {
            if (!IsSecure) {
                return null;
            }

            bool flag = _listener.SslConfiguration.ServerCertificate != null;
            bool flag2 = EndPointListener.CertificateExists(Port, _listener.CertificateFolderPath);

            if (flag && flag2) {
                Log.Warn("The server certificate associated with the port number already exists.");
                return null;
            }

            return flag || flag2 ? null : "The secure connection requires a server certificate.";
        }

        void init(string hostname, IPAddress address, int port, bool secure) {
            _hostname = hostname ?? address.ToString();
            Address = address;
            Port = port;
            IsSecure = secure;
            _listener = new HttpListener();
            _listener.Prefixes.Add(string.Format("http{0}://{1}:{2}/", !secure ? string.Empty : "s", _hostname, port));
            Log = _listener.Log;
            WebSocketServices = new WebSocketServiceManager(Log);
            _sync = new object();
            OperatingSystem oSVersion = Environment.OSVersion;
            _windows = oSVersion.Platform != PlatformID.Unix && oSVersion.Platform != PlatformID.MacOSX;
        }

        void processRequest(HttpListenerContext context) {
            object obj;

            switch (context.Request.HttpMethod) {
                case "GET":
                    obj = OnGet;
                    break;

                case "HEAD":
                    obj = OnHead;
                    break;

                case "POST":
                    obj = OnPost;
                    break;

                case "PUT":
                    obj = OnPut;
                    break;

                case "DELETE":
                    obj = OnDelete;
                    break;

                case "OPTIONS":
                    obj = OnOptions;
                    break;

                case "TRACE":
                    obj = OnTrace;
                    break;

                case "CONNECT":
                    obj = OnConnect;
                    break;

                case "PATCH":
                    obj = OnPatch;
                    break;

                default:
                    obj = null;
                    break;
            }

            EventHandler<HttpRequestEventArgs> eventHandler = (EventHandler<HttpRequestEventArgs>)obj;

            if (eventHandler != null) {
                eventHandler(this, new HttpRequestEventArgs(context));
            } else {
                context.Response.StatusCode = 501;
            }

            context.Response.Close();
        }

        void processRequest(HttpListenerWebSocketContext context) {
            WebSocketServiceHost host;

            if (!WebSocketServices.InternalTryGetServiceHost(context.RequestUri.AbsolutePath, out host)) {
                context.Close(HttpStatusCode.NotImplemented);
            } else {
                host.StartSession(context);
            }
        }

        void receiveRequest() {
            while (true) {
                try {
                    HttpListenerContext ctx = _listener.GetContext();

                    ThreadPool.QueueUserWorkItem(delegate {
                        try {
                            if (ctx.Request.IsUpgradeTo("websocket")) {
                                processRequest(ctx.AcceptWebSocket(null));
                            } else {
                                processRequest(ctx);
                            }
                        } catch (Exception ex3) {
                            Log.Fatal(ex3.ToString());
                            ctx.Connection.Close(true);
                        }
                    });
                } catch (HttpListenerException ex) {
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
            _listener.Start();
            _receiveThread = new Thread(receiveRequest);
            _receiveThread.IsBackground = true;
            _receiveThread.Start();
        }

        void stopReceiving(int millisecondsTimeout) {
            _listener.Close();
            _receiveThread.Join(millisecondsTimeout);
        }

        static bool tryCreateUri(string uriString, out Uri result, out string message) {
            result = null;
            Uri uri = uriString.ToUri();

            if (uri == null) {
                message = "An invalid URI string: " + uriString;
                return false;
            }

            if (!uri.IsAbsoluteUri) {
                message = "Not an absolute URI: " + uriString;
                return false;
            }

            string scheme = uri.Scheme;

            if (!(scheme == "http") && !(scheme == "https")) {
                message = "The scheme part isn't 'http' or 'https': " + uriString;
                return false;
            }

            if (uri.PathAndQuery != "/") {
                message = "Includes the path or query component: " + uriString;
                return false;
            }

            if (uri.Fragment.Length > 0) {
                message = "Includes the fragment component: " + uriString;
                return false;
            }

            if (uri.Port == 0) {
                message = "The port part is zero: " + uriString;
                return false;
            }

            result = uri;
            message = string.Empty;
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

        public byte[] GetFile(string path) {
            path = RootPath + path;

            if (_windows) {
                path = path.Replace("/", "\\");
            }

            return !File.Exists(path) ? null : File.ReadAllBytes(path);
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

            WebSocketServices.Stop(new CloseEventArgs(), true, true);
            stopReceiving(5000);
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

            if (code == 1005) {
                WebSocketServices.Stop(new CloseEventArgs(), true, true);
            } else {
                bool flag = !code.IsReserved();
                WebSocketServices.Stop(new CloseEventArgs(code, reason), flag, flag);
            }

            stopReceiving(5000);
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

            if (code == CloseStatusCode.NoStatus) {
                WebSocketServices.Stop(new CloseEventArgs(), true, true);
            } else {
                bool flag = !code.IsReserved();
                WebSocketServices.Stop(new CloseEventArgs(code, reason), flag, flag);
            }

            stopReceiving(5000);
            _state = ServerState.Stop;
        }
    }
}