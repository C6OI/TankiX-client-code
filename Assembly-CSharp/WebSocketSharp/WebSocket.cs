using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;

namespace WebSocketSharp {
    public class WebSocket : IDisposable {
        const string _guid = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

        const string _version = "13";

        internal static readonly byte[] EmptyBytes;

        internal static readonly int FragmentLength;

        internal static readonly RandomNumberGenerator RandomNumber;
        AuthenticationChallenge _authChallenge;

        string _base64Key;

        readonly bool _client;

        Action _closeContext;

        CompressionMethod _compression;

        WebSocketContext _context;

        bool _enableRedirection;

        AutoResetEvent _exitReceiving;

        string _extensions;

        bool _extensionsRequested;

        object _forConn;

        object _forMessageEventQueue;

        object _forSend;

        MemoryStream _fragmentsBuffer;

        bool _fragmentsCompressed;

        Opcode _fragmentsOpcode;

        bool _inContinuation;

        volatile bool _inMessage;

        volatile Logger _logger;

        readonly Action<MessageEventArgs> _message;

        Queue<MessageEventArgs> _messageEventQueue;

        uint _nonceCount;

        string _origin;

        bool _preAuth;

        string _protocol;

        readonly string[] _protocols;

        bool _protocolsRequested;

        NetworkCredential _proxyCredentials;

        Uri _proxyUri;

        volatile WebSocketState _readyState;

        AutoResetEvent _receivePong;

        ClientSslConfiguration _sslConfig;

        Stream _stream;

        TcpClient _tcpClient;

        Uri _uri;

        TimeSpan _waitTime;

        static WebSocket() {
            EmptyBytes = new byte[0];
            FragmentLength = 1016;
            RandomNumber = new RNGCryptoServiceProvider();
        }

        internal WebSocket(HttpListenerWebSocketContext context, string protocol) {
            _context = context;
            _protocol = protocol;
            _closeContext = context.Close;
            _logger = context.Log;
            _message = messages;
            IsSecure = context.IsSecureConnection;
            _stream = context.Stream;
            _waitTime = TimeSpan.FromSeconds(1.0);
            init();
        }

        internal WebSocket(TcpListenerWebSocketContext context, string protocol) {
            _context = context;
            _protocol = protocol;
            _closeContext = context.Close;
            _logger = context.Log;
            _message = messages;
            IsSecure = context.IsSecureConnection;
            _stream = context.Stream;
            _waitTime = TimeSpan.FromSeconds(1.0);
            init();
        }

        public WebSocket(string url, params string[] protocols) {
            if (url == null) {
                throw new ArgumentNullException("url");
            }

            if (url.Length == 0) {
                throw new ArgumentException("An empty string.", "url");
            }

            string text;

            if (!url.TryCreateWebSocketUri(out _uri, out text)) {
                throw new ArgumentException(text, "url");
            }

            if (protocols != null && protocols.Length > 0) {
                text = protocols.CheckIfValidProtocols();

                if (text != null) {
                    throw new ArgumentException(text, "protocols");
                }

                _protocols = protocols;
            }

            _base64Key = CreateBase64Key();
            _client = true;
            _logger = new Logger();
            _message = messagec;
            IsSecure = _uri.Scheme == "wss";
            _waitTime = TimeSpan.FromSeconds(5.0);
            init();
        }

        internal CookieCollection CookieCollection { get; private set; }

        internal Func<WebSocketContext, string> CustomHandshakeRequestChecker { get; set; }

        internal bool HasMessage {
            get {
                lock (_forMessageEventQueue) {
                    return _messageEventQueue.Count > 0;
                }
            }
        }

        internal bool IgnoreExtensions { get; set; }

        internal bool IsConnected => _readyState == WebSocketState.Open || _readyState == WebSocketState.Closing;

        public CompressionMethod Compression {
            get => _compression;
            set {
                lock (_forConn) {
                    string text;

                    if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                        _logger.Error(text);
                        error("An error has occurred in setting the compression.", null);
                    } else {
                        _compression = value;
                    }
                }
            }
        }

        public IEnumerable<Cookie> Cookies {
            get {
                lock (CookieCollection.SyncRoot) {
                    IEnumerator enumerator = CookieCollection.GetEnumerator();

                    try {
                        while (enumerator.MoveNext()) {
                            yield return (Cookie)enumerator.Current;
                        }
                    } finally {
                        IDisposable disposable;
                        IDisposable disposable2 = disposable = enumerator as IDisposable;

                        if (disposable != null) {
                            disposable2.Dispose();
                        }
                    }
                }
            }
        }

        public NetworkCredential Credentials { get; private set; }

        public bool EmitOnPing { get; set; }

        public bool EnableRedirection {
            get => _enableRedirection;
            set {
                lock (_forConn) {
                    string text;

                    if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                        _logger.Error(text);
                        error("An error has occurred in setting the enable redirection.", null);
                    } else {
                        _enableRedirection = value;
                    }
                }
            }
        }

        public string Extensions => _extensions ?? string.Empty;

        public bool IsAlive => Ping();

        public bool IsSecure { get; private set; }

        public Logger Log {
            get => _logger;
            internal set => _logger = value;
        }

        public string Origin {
            get => _origin;
            set {
                lock (_forConn) {
                    string text;
                    Uri result;

                    if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                        _logger.Error(text);
                        error("An error has occurred in setting the origin.", null);
                    } else if (value.IsNullOrEmpty()) {
                        _origin = value;
                    } else if (!Uri.TryCreate(value, UriKind.Absolute, out result) || result.Segments.Length > 1) {
                        _logger.Error("The syntax of an origin must be '<scheme>://<host>[:<port>]'.");
                        error("An error has occurred in setting the origin.", null);
                    } else {
                        _origin = value.TrimEnd('/');
                    }
                }
            }
        }

        public string Protocol {
            get => _protocol ?? string.Empty;
            internal set => _protocol = value;
        }

        public WebSocketState ReadyState => _readyState;

        public ClientSslConfiguration SslConfiguration {
            get => !_client ? null : _sslConfig ?? (_sslConfig = new ClientSslConfiguration(_uri.DnsSafeHost));
            set {
                lock (_forConn) {
                    string text;

                    if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                        _logger.Error(text);
                        error("An error has occurred in setting the ssl configuration.", null);
                    } else {
                        _sslConfig = value;
                    }
                }
            }
        }

        public Uri Url => !_client ? _context.RequestUri : _uri;

        public TimeSpan WaitTime {
            get => _waitTime;
            set {
                lock (_forConn) {
                    string text;

                    if (!checkIfAvailable(true, true, true, false, false, true, out text) || !value.CheckWaitTime(out text)) {
                        _logger.Error(text);
                        error("An error has occurred in setting the wait time.", null);
                    } else {
                        _waitTime = value;
                    }
                }
            }
        }

        void IDisposable.Dispose() {
            close(new CloseEventArgs(CloseStatusCode.Away), true, true, false);
        }

        public event EventHandler<CloseEventArgs> OnClose;

        public event EventHandler<ErrorEventArgs> OnError;

        public event EventHandler<MessageEventArgs> OnMessage;

        public event EventHandler OnOpen;

        bool accept() {
            lock (_forConn) {
                string text;

                if (!checkIfAvailable(true, false, false, false, out text)) {
                    _logger.Error(text);
                    error("An error has occurred in accepting.", null);
                    return false;
                }

                try {
                    if (!acceptHandshake()) {
                        return false;
                    }

                    _readyState = WebSocketState.Open;
                } catch (Exception ex) {
                    _logger.Fatal(ex.ToString());
                    fatal("An exception has occurred while accepting.", ex);
                    return false;
                }

                return true;
            }
        }

        bool acceptHandshake() {
            _logger.Debug(string.Format("A request from {0}:\n{1}", _context.UserEndPoint, _context));
            string text;

            if (!checkHandshakeRequest(_context, out text)) {
                sendHttpResponse(createHandshakeFailureResponse(HttpStatusCode.BadRequest));
                _logger.Fatal(text);
                fatal("An error has occurred while accepting.", CloseStatusCode.ProtocolError);
                return false;
            }

            if (!customCheckHandshakeRequest(_context, out text)) {
                sendHttpResponse(createHandshakeFailureResponse(HttpStatusCode.BadRequest));
                _logger.Fatal(text);
                fatal("An error has occurred while accepting.", CloseStatusCode.PolicyViolation);
                return false;
            }

            _base64Key = _context.Headers["Sec-WebSocket-Key"];

            if (_protocol != null) {
                processSecWebSocketProtocolHeader(_context.SecWebSocketProtocols);
            }

            if (!IgnoreExtensions) {
                processSecWebSocketExtensionsClientHeader(_context.Headers["Sec-WebSocket-Extensions"]);
            }

            return sendHttpResponse(createHandshakeResponse());
        }

        bool checkHandshakeRequest(WebSocketContext context, out string message) {
            message = null;

            if (context.RequestUri == null) {
                message = "Specifies an invalid Request-URI.";
                return false;
            }

            if (!context.IsWebSocketRequest) {
                message = "Not a WebSocket handshake request.";
                return false;
            }

            NameValueCollection headers = context.Headers;

            if (!validateSecWebSocketKeyHeader(headers["Sec-WebSocket-Key"])) {
                message = "Includes no Sec-WebSocket-Key header, or it has an invalid value.";
                return false;
            }

            if (!validateSecWebSocketVersionClientHeader(headers["Sec-WebSocket-Version"])) {
                message = "Includes no Sec-WebSocket-Version header, or it has an invalid value.";
                return false;
            }

            if (!validateSecWebSocketProtocolClientHeader(headers["Sec-WebSocket-Protocol"])) {
                message = "Includes an invalid Sec-WebSocket-Protocol header.";
                return false;
            }

            if (!IgnoreExtensions && !validateSecWebSocketExtensionsClientHeader(headers["Sec-WebSocket-Extensions"])) {
                message = "Includes an invalid Sec-WebSocket-Extensions header.";
                return false;
            }

            return true;
        }

        bool checkHandshakeResponse(HttpResponse response, out string message) {
            message = null;

            if (response.IsRedirect) {
                message = "Indicates the redirection.";
                return false;
            }

            if (response.IsUnauthorized) {
                message = "Requires the authentication.";
                return false;
            }

            if (!response.IsWebSocketResponse) {
                message = "Not a WebSocket handshake response.";
                return false;
            }

            NameValueCollection headers = response.Headers;

            if (!validateSecWebSocketAcceptHeader(headers["Sec-WebSocket-Accept"])) {
                message = "Includes no Sec-WebSocket-Accept header, or it has an invalid value.";
                return false;
            }

            if (!validateSecWebSocketProtocolServerHeader(headers["Sec-WebSocket-Protocol"])) {
                message = "Includes no Sec-WebSocket-Protocol header, or it has an invalid value.";
                return false;
            }

            if (!validateSecWebSocketExtensionsServerHeader(headers["Sec-WebSocket-Extensions"])) {
                message = "Includes an invalid Sec-WebSocket-Extensions header.";
                return false;
            }

            if (!validateSecWebSocketVersionServerHeader(headers["Sec-WebSocket-Version"])) {
                message = "Includes an invalid Sec-WebSocket-Version header.";
                return false;
            }

            return true;
        }

        bool checkIfAvailable(bool connecting, bool open, bool closing, bool closed, out string message) {
            message = null;

            if (!connecting && _readyState == WebSocketState.Connecting) {
                message = "This operation isn't available in: connecting";
                return false;
            }

            if (!open && _readyState == WebSocketState.Open) {
                message = "This operation isn't available in: open";
                return false;
            }

            if (!closing && _readyState == WebSocketState.Closing) {
                message = "This operation isn't available in: closing";
                return false;
            }

            if (!closed && _readyState == WebSocketState.Closed) {
                message = "This operation isn't available in: closed";
                return false;
            }

            return true;
        }

        bool checkIfAvailable(bool client, bool server, bool connecting, bool open, bool closing, bool closed, out string message) {
            message = null;

            if (!client && _client) {
                message = "This operation isn't available in: client";
                return false;
            }

            if (!server && !_client) {
                message = "This operation isn't available in: server";
                return false;
            }

            return checkIfAvailable(connecting, open, closing, closed, out message);
        }

        bool checkReceivedFrame(WebSocketFrame frame, out string message) {
            message = null;
            bool isMasked = frame.IsMasked;

            if (_client && isMasked) {
                message = "A frame from the server is masked.";
                return false;
            }

            if (!_client && !isMasked) {
                message = "A frame from a client isn't masked.";
                return false;
            }

            if (_inContinuation && frame.IsData) {
                message = "A data frame has been received while receiving continuation frames.";
                return false;
            }

            if (frame.IsCompressed && _compression == CompressionMethod.None) {
                message = "A compressed frame has been received without any agreement for it.";
                return false;
            }

            if (frame.Rsv2 == Rsv.On) {
                message = "The RSV2 of a frame is non-zero without any negotiation for it.";
                return false;
            }

            if (frame.Rsv3 == Rsv.On) {
                message = "The RSV3 of a frame is non-zero without any negotiation for it.";
                return false;
            }

            return true;
        }

        void close(CloseEventArgs e, bool send, bool receive, bool received) {
            lock (_forConn) {
                if (_readyState == WebSocketState.Closing) {
                    _logger.Info("The closing is already in progress.");
                    return;
                }

                if (_readyState == WebSocketState.Closed) {
                    _logger.Info("The connection has been closed.");
                    return;
                }

                send = send && _readyState == WebSocketState.Open;
                receive = receive && send;
                _readyState = WebSocketState.Closing;
            }

            _logger.Trace("Begin closing the connection.");
            byte[] frameAsBytes = !send ? null : WebSocketFrame.CreateCloseFrame(e.PayloadData, _client).ToArray();
            e.WasClean = closeHandshake(frameAsBytes, receive, received);
            releaseResources();
            _logger.Trace("End closing the connection.");
            _readyState = WebSocketState.Closed;

            try {
                OnClose.Emit(this, e);
            } catch (Exception ex) {
                _logger.Error(ex.ToString());
                error("An exception has occurred during the OnClose event.", ex);
            }
        }

        void closeAsync(CloseEventArgs e, bool send, bool receive, bool received) {
            Action<CloseEventArgs, bool, bool, bool> closer = close;

            closer.BeginInvoke(e, send, receive, received, delegate(IAsyncResult ar) {
                closer.EndInvoke(ar);
            }, null);
        }

        bool closeHandshake(byte[] frameAsBytes, bool receive, bool received) {
            bool flag = frameAsBytes != null && sendBytes(frameAsBytes);
            received = received || receive && flag && _exitReceiving != null && _exitReceiving.WaitOne(_waitTime);
            bool flag2 = flag && received;
            _logger.Debug(string.Format("Was clean?: {0}\n  sent: {1}\n  received: {2}", flag2, flag, received));
            return flag2;
        }

        bool connect() {
            lock (_forConn) {
                string text;

                if (!checkIfAvailable(true, false, false, true, out text)) {
                    _logger.Error(text);
                    error("An error has occurred in connecting.", null);
                    return false;
                }

                try {
                    _readyState = WebSocketState.Connecting;

                    if (!doHandshake()) {
                        return false;
                    }

                    _readyState = WebSocketState.Open;
                } catch (Exception ex) {
                    _logger.Fatal(ex.ToString());
                    fatal("An exception has occurred while connecting.", ex);
                    return false;
                }

                return true;
            }
        }

        string createExtensions() {
            StringBuilder stringBuilder = new(80);

            if (_compression != 0) {
                string arg = _compression.ToExtensionString("server_no_context_takeover", "client_no_context_takeover");
                stringBuilder.AppendFormat("{0}, ", arg);
            }

            int length = stringBuilder.Length;

            if (length > 2) {
                stringBuilder.Length = length - 2;
                return stringBuilder.ToString();
            }

            return null;
        }

        HttpResponse createHandshakeFailureResponse(HttpStatusCode code) {
            HttpResponse httpResponse = HttpResponse.CreateCloseResponse(code);
            httpResponse.Headers["Sec-WebSocket-Version"] = "13";
            return httpResponse;
        }

        HttpRequest createHandshakeRequest() {
            HttpRequest httpRequest = HttpRequest.CreateWebSocketRequest(_uri);
            NameValueCollection headers = httpRequest.Headers;

            if (!_origin.IsNullOrEmpty()) {
                headers["Origin"] = _origin;
            }

            headers["Sec-WebSocket-Key"] = _base64Key;
            _protocolsRequested = _protocols != null;

            if (_protocolsRequested) {
                headers["Sec-WebSocket-Protocol"] = _protocols.ToString(", ");
            }

            _extensionsRequested = _compression != CompressionMethod.None;

            if (_extensionsRequested) {
                headers["Sec-WebSocket-Extensions"] = createExtensions();
            }

            headers["Sec-WebSocket-Version"] = "13";
            AuthenticationResponse authenticationResponse = null;

            if (_authChallenge != null && Credentials != null) {
                authenticationResponse = new AuthenticationResponse(_authChallenge, Credentials, _nonceCount);
                _nonceCount = authenticationResponse.NonceCount;
            } else if (_preAuth) {
                authenticationResponse = new AuthenticationResponse(Credentials);
            }

            if (authenticationResponse != null) {
                headers["Authorization"] = authenticationResponse.ToString();
            }

            if (CookieCollection.Count > 0) {
                httpRequest.SetCookies(CookieCollection);
            }

            return httpRequest;
        }

        HttpResponse createHandshakeResponse() {
            HttpResponse httpResponse = HttpResponse.CreateWebSocketResponse();
            NameValueCollection headers = httpResponse.Headers;
            headers["Sec-WebSocket-Accept"] = CreateResponseKey(_base64Key);

            if (_protocol != null) {
                headers["Sec-WebSocket-Protocol"] = _protocol;
            }

            if (_extensions != null) {
                headers["Sec-WebSocket-Extensions"] = _extensions;
            }

            if (CookieCollection.Count > 0) {
                httpResponse.SetCookies(CookieCollection);
            }

            return httpResponse;
        }

        bool customCheckHandshakeRequest(WebSocketContext context, out string message) {
            message = null;
            return CustomHandshakeRequestChecker == null || (message = CustomHandshakeRequestChecker(context)) == null;
        }

        MessageEventArgs dequeueFromMessageEventQueue() {
            lock (_forMessageEventQueue) {
                return _messageEventQueue.Count <= 0 ? null : _messageEventQueue.Dequeue();
            }
        }

        bool doHandshake() {
            setClientStream();
            HttpResponse httpResponse = sendHandshakeRequest();
            string text;

            if (!checkHandshakeResponse(httpResponse, out text)) {
                _logger.Fatal(text);
                fatal("An error has occurred while connecting.", CloseStatusCode.ProtocolError);
                return false;
            }

            if (_protocolsRequested) {
                _protocol = httpResponse.Headers["Sec-WebSocket-Protocol"];
            }

            if (_extensionsRequested) {
                processSecWebSocketExtensionsServerHeader(httpResponse.Headers["Sec-WebSocket-Extensions"]);
            }

            processCookies(httpResponse.Cookies);
            return true;
        }

        void enqueueToMessageEventQueue(MessageEventArgs e) {
            lock (_forMessageEventQueue) {
                _messageEventQueue.Enqueue(e);
            }
        }

        void error(string message, Exception exception) {
            try {
                OnError.Emit(this, new ErrorEventArgs(message, exception));
            } catch (Exception ex) {
                _logger.Error(ex.ToString());
            }
        }

        void fatal(string message, Exception exception) {
            CloseStatusCode code = !(exception is WebSocketException) ? CloseStatusCode.Abnormal : ((WebSocketException)exception).Code;
            fatal(message, code);
        }

        void fatal(string message, CloseStatusCode code) {
            close(new CloseEventArgs(code, message), !code.IsReserved(), false, false);
        }

        void init() {
            _compression = CompressionMethod.None;
            CookieCollection = new CookieCollection();
            _forConn = new object();
            _forSend = new object();
            _messageEventQueue = new Queue<MessageEventArgs>();
            _forMessageEventQueue = ((ICollection)_messageEventQueue).SyncRoot;
            _readyState = WebSocketState.Connecting;
        }

        void message() {
            MessageEventArgs obj = null;

            lock (_forMessageEventQueue) {
                if (_inMessage || _messageEventQueue.Count == 0 || _readyState != WebSocketState.Open) {
                    return;
                }

                _inMessage = true;
                obj = _messageEventQueue.Dequeue();
            }

            _message(obj);
        }

        void messagec(MessageEventArgs e) {
            while (true) {
                try {
                    OnMessage.Emit(this, e);
                } catch (Exception ex) {
                    _logger.Error(ex.ToString());
                    error("An exception has occurred during an OnMessage event.", ex);
                }

                lock (_forMessageEventQueue) {
                    if (_messageEventQueue.Count == 0 || _readyState != WebSocketState.Open) {
                        _inMessage = false;
                        break;
                    }

                    e = _messageEventQueue.Dequeue();
                }
            }
        }

        void messages(MessageEventArgs e) {
            try {
                OnMessage.Emit(this, e);
            } catch (Exception ex) {
                _logger.Error(ex.ToString());
                error("An exception has occurred during an OnMessage event.", ex);
            }

            lock (_forMessageEventQueue) {
                if (_messageEventQueue.Count == 0 || _readyState != WebSocketState.Open) {
                    _inMessage = false;
                    return;
                }

                e = _messageEventQueue.Dequeue();
            }

            ThreadPool.QueueUserWorkItem(delegate {
                messages(e);
            });
        }

        void open() {
            _inMessage = true;
            startReceiving();

            try {
                OnOpen.Emit(this, EventArgs.Empty);
            } catch (Exception ex) {
                _logger.Error(ex.ToString());
                error("An exception has occurred during the OnOpen event.", ex);
            }

            MessageEventArgs obj = null;

            lock (_forMessageEventQueue) {
                if (_messageEventQueue.Count == 0 || _readyState != WebSocketState.Open) {
                    _inMessage = false;
                    return;
                }

                obj = _messageEventQueue.Dequeue();
            }

            _message.BeginInvoke(obj, delegate(IAsyncResult ar) {
                _message.EndInvoke(ar);
            }, null);
        }

        bool processCloseFrame(WebSocketFrame frame) {
            PayloadData payloadData = frame.PayloadData;
            close(new CloseEventArgs(payloadData), !payloadData.IncludesReservedCloseStatusCode, false, true);
            return false;
        }

        void processCookies(CookieCollection cookies) {
            if (cookies.Count != 0) {
                CookieCollection.SetOrRemove(cookies);
            }
        }

        bool processDataFrame(WebSocketFrame frame) {
            enqueueToMessageEventQueue(!frame.IsCompressed ? new MessageEventArgs(frame) : new MessageEventArgs(frame.Opcode, frame.PayloadData.ApplicationData.Decompress(_compression)));
            return true;
        }

        bool processFragmentFrame(WebSocketFrame frame) {
            if (!_inContinuation) {
                if (frame.IsContinuation) {
                    return true;
                }

                _fragmentsOpcode = frame.Opcode;
                _fragmentsCompressed = frame.IsCompressed;
                _fragmentsBuffer = new MemoryStream();
                _inContinuation = true;
            }

            _fragmentsBuffer.WriteBytes(frame.PayloadData.ApplicationData, 1024);

            if (frame.IsFinal) {
                using (_fragmentsBuffer) {
                    byte[] rawData = !_fragmentsCompressed ? _fragmentsBuffer.ToArray() : _fragmentsBuffer.DecompressToArray(_compression);
                    enqueueToMessageEventQueue(new MessageEventArgs(_fragmentsOpcode, rawData));
                }

                _fragmentsBuffer = null;
                _inContinuation = false;
            }

            return true;
        }

        bool processPingFrame(WebSocketFrame frame) {
            if (send(new WebSocketFrame(Opcode.Pong, frame.PayloadData, _client).ToArray())) {
                _logger.Trace("Returned a pong.");
            }

            if (EmitOnPing) {
                enqueueToMessageEventQueue(new MessageEventArgs(frame));
            }

            return true;
        }

        bool processPongFrame(WebSocketFrame frame) {
            _receivePong.Set();
            _logger.Trace("Received a pong.");
            return true;
        }

        bool processReceivedFrame(WebSocketFrame frame) {
            string text;

            if (!checkReceivedFrame(frame, out text)) {
                throw new WebSocketException(CloseStatusCode.ProtocolError, text);
            }

            frame.Unmask();

            return frame.IsFragment ? processFragmentFrame(frame) :
                   frame.IsData ? processDataFrame(frame) :
                   frame.IsPing ? processPingFrame(frame) :
                   frame.IsPong ? processPongFrame(frame) :
                   !frame.IsClose ? processUnsupportedFrame(frame) : processCloseFrame(frame);
        }

        void processSecWebSocketExtensionsClientHeader(string value) {
            if (value == null) {
                return;
            }

            StringBuilder stringBuilder = new(80);
            bool flag = false;

            foreach (string item in value.SplitHeaderValue(',')) {
                string value2 = item.Trim();

                if (!flag && value2.IsCompressionExtension(CompressionMethod.Deflate)) {
                    _compression = CompressionMethod.Deflate;
                    stringBuilder.AppendFormat("{0}, ", _compression.ToExtensionString("client_no_context_takeover", "server_no_context_takeover"));
                    flag = true;
                }
            }

            int length = stringBuilder.Length;

            if (length > 2) {
                stringBuilder.Length = length - 2;
                _extensions = stringBuilder.ToString();
            }
        }

        void processSecWebSocketExtensionsServerHeader(string value) {
            if (value == null) {
                _compression = CompressionMethod.None;
            } else {
                _extensions = value;
            }
        }

        void processSecWebSocketProtocolHeader(IEnumerable<string> values) {
            if (!values.Contains(p => p == _protocol)) {
                _protocol = null;
            }
        }

        bool processUnsupportedFrame(WebSocketFrame frame) {
            _logger.Fatal("An unsupported frame:" + frame.PrintToString(false));
            fatal("There is no way to handle it.", CloseStatusCode.PolicyViolation);
            return false;
        }

        void releaseClientResources() {
            if (_stream != null) {
                _stream.Dispose();
                _stream = null;
            }

            if (_tcpClient != null) {
                _tcpClient.Close();
                _tcpClient = null;
            }
        }

        void releaseCommonResources() {
            if (_fragmentsBuffer != null) {
                _fragmentsBuffer.Dispose();
                _fragmentsBuffer = null;
                _inContinuation = false;
            }

            if (_receivePong != null) {
                _receivePong.Close();
                _receivePong = null;
            }

            if (_exitReceiving != null) {
                _exitReceiving.Close();
                _exitReceiving = null;
            }
        }

        void releaseResources() {
            if (_client) {
                releaseClientResources();
            } else {
                releaseServerResources();
            }

            releaseCommonResources();
        }

        void releaseServerResources() {
            if (_closeContext != null) {
                _closeContext();
                _closeContext = null;
                _stream = null;
                _context = null;
            }
        }

        bool send(byte[] frameAsBytes) {
            lock (_forConn) {
                if (_readyState != WebSocketState.Open) {
                    _logger.Error("The sending has been interrupted.");
                    return false;
                }

                return sendBytes(frameAsBytes);
            }
        }

        bool send(Opcode opcode, Stream stream) {
            lock (_forSend) {
                Stream stream2 = stream;
                bool flag = false;
                bool flag2 = false;

                try {
                    if (_compression != 0) {
                        stream = stream.Compress(_compression);
                        flag = true;
                    }

                    flag2 = send(opcode, stream, flag);

                    if (!flag2) {
                        error("The sending has been interrupted.", null);
                    }
                } catch (Exception ex) {
                    _logger.Error(ex.ToString());
                    error("An exception has occurred while sending data.", ex);
                } finally {
                    if (flag) {
                        stream.Dispose();
                    }

                    stream2.Dispose();
                }

                return flag2;
            }
        }

        bool send(Opcode opcode, Stream stream, bool compressed) {
            long length = stream.Length;

            if (length == 0) {
                return send(Fin.Final, opcode, EmptyBytes, compressed);
            }

            long num = length / FragmentLength;
            int num2 = (int)(length % FragmentLength);
            byte[] array = null;

            if (num == 0) {
                array = new byte[num2];
                return stream.Read(array, 0, num2) == num2 && send(Fin.Final, opcode, array, compressed);
            }

            array = new byte[FragmentLength];

            if (num == 1 && num2 == 0) {
                return stream.Read(array, 0, FragmentLength) == FragmentLength && send(Fin.Final, opcode, array, compressed);
            }

            if (stream.Read(array, 0, FragmentLength) != FragmentLength || !send(Fin.More, opcode, array, compressed)) {
                return false;
            }

            long num3 = num2 != 0 ? num - 1 : num - 2;

            for (long num4 = 0L; num4 < num3; num4++) {
                if (stream.Read(array, 0, FragmentLength) != FragmentLength || !send(Fin.More, Opcode.Cont, array, compressed)) {
                    return false;
                }
            }

            if (num2 == 0) {
                num2 = FragmentLength;
            } else {
                array = new byte[num2];
            }

            return stream.Read(array, 0, num2) == num2 && send(Fin.Final, Opcode.Cont, array, compressed);
        }

        bool send(Fin fin, Opcode opcode, byte[] data, bool compressed) {
            lock (_forConn) {
                if (_readyState != WebSocketState.Open) {
                    _logger.Error("The sending has been interrupted.");
                    return false;
                }

                return sendBytes(new WebSocketFrame(fin, opcode, data, compressed, _client).ToArray());
            }
        }

        void sendAsync(Opcode opcode, Stream stream, Action<bool> completed) {
            Func<Opcode, Stream, bool> sender = send;

            sender.BeginInvoke(opcode, stream, delegate(IAsyncResult ar) {
                try {
                    bool obj = sender.EndInvoke(ar);

                    if (completed != null) {
                        completed(obj);
                    }
                } catch (Exception ex) {
                    _logger.Error(ex.ToString());
                    error("An exception has occurred during a send callback.", ex);
                }
            }, null);
        }

        bool sendBytes(byte[] bytes) {
            try {
                _stream.Write(bytes, 0, bytes.Length);
                return true;
            } catch (Exception ex) {
                _logger.Error(ex.ToString());
                return false;
            }
        }

        HttpResponse sendHandshakeRequest() {
            HttpRequest httpRequest = createHandshakeRequest();
            HttpResponse httpResponse = sendHttpRequest(httpRequest, 90000);

            if (httpResponse.IsUnauthorized) {
                string text = httpResponse.Headers["WWW-Authenticate"];
                _logger.Warn(string.Format("Received an authentication requirement for '{0}'.", text));

                if (text.IsNullOrEmpty()) {
                    _logger.Error("No authentication challenge is specified.");
                    return httpResponse;
                }

                _authChallenge = AuthenticationChallenge.Parse(text);

                if (_authChallenge == null) {
                    _logger.Error("An invalid authentication challenge is specified.");
                    return httpResponse;
                }

                if (Credentials != null && (!_preAuth || _authChallenge.Scheme == AuthenticationSchemes.Digest)) {
                    if (httpResponse.HasConnectionClose) {
                        releaseClientResources();
                        setClientStream();
                    }

                    AuthenticationResponse authenticationResponse = new(_authChallenge, Credentials, _nonceCount);
                    _nonceCount = authenticationResponse.NonceCount;
                    httpRequest.Headers["Authorization"] = authenticationResponse.ToString();
                    httpResponse = sendHttpRequest(httpRequest, 15000);
                }
            }

            if (httpResponse.IsRedirect) {
                string text2 = httpResponse.Headers["Location"];
                _logger.Warn(string.Format("Received a redirection to '{0}'.", text2));

                if (_enableRedirection) {
                    if (text2.IsNullOrEmpty()) {
                        _logger.Error("No url to redirect is located.");
                        return httpResponse;
                    }

                    Uri result;
                    string text3;

                    if (!text2.TryCreateWebSocketUri(out result, out text3)) {
                        _logger.Error("An invalid url to redirect is located: " + text3);
                        return httpResponse;
                    }

                    releaseClientResources();
                    _uri = result;
                    IsSecure = result.Scheme == "wss";
                    setClientStream();
                    return sendHandshakeRequest();
                }
            }

            return httpResponse;
        }

        HttpResponse sendHttpRequest(HttpRequest request, int millisecondsTimeout) {
            _logger.Debug("A request to the server:\n" + request);
            HttpResponse response = request.GetResponse(_stream, millisecondsTimeout);
            _logger.Debug("A response to this request:\n" + response);
            return response;
        }

        bool sendHttpResponse(HttpResponse response) {
            _logger.Debug("A response to this request:\n" + response);
            return sendBytes(response.ToByteArray());
        }

        void sendProxyConnectRequest() {
            HttpRequest httpRequest = HttpRequest.CreateConnectRequest(_uri);
            HttpResponse httpResponse = sendHttpRequest(httpRequest, 90000);

            if (httpResponse.IsProxyAuthenticationRequired) {
                string text = httpResponse.Headers["Proxy-Authenticate"];
                _logger.Warn(string.Format("Received a proxy authentication requirement for '{0}'.", text));

                if (text.IsNullOrEmpty()) {
                    throw new WebSocketException("No proxy authentication challenge is specified.");
                }

                AuthenticationChallenge authenticationChallenge = AuthenticationChallenge.Parse(text);

                if (authenticationChallenge == null) {
                    throw new WebSocketException("An invalid proxy authentication challenge is specified.");
                }

                if (_proxyCredentials != null) {
                    if (httpResponse.HasConnectionClose) {
                        releaseClientResources();
                        _tcpClient = new TcpClient(_proxyUri.DnsSafeHost, _proxyUri.Port);
                        _stream = _tcpClient.GetStream();
                    }

                    AuthenticationResponse authenticationResponse = new(authenticationChallenge, _proxyCredentials, 0u);
                    httpRequest.Headers["Proxy-Authorization"] = authenticationResponse.ToString();
                    httpResponse = sendHttpRequest(httpRequest, 15000);
                }

                if (httpResponse.IsProxyAuthenticationRequired) {
                    throw new WebSocketException("A proxy authentication is required.");
                }
            }

            if (httpResponse.StatusCode[0] != '2') {
                throw new WebSocketException("The proxy has failed a connection to the requested host and port.");
            }
        }

        void setClientStream() {
            if (_proxyUri != null) {
                _tcpClient = new TcpClient(_proxyUri.DnsSafeHost, _proxyUri.Port);
                _stream = _tcpClient.GetStream();
                sendProxyConnectRequest();
            } else {
                _tcpClient = new TcpClient(_uri.DnsSafeHost, _uri.Port);
                _stream = _tcpClient.GetStream();
            }

            if (IsSecure) {
                ClientSslConfiguration sslConfiguration = SslConfiguration;
                string targetHost = sslConfiguration.TargetHost;

                if (targetHost != _uri.DnsSafeHost) {
                    throw new WebSocketException(CloseStatusCode.TlsHandshakeFailure, "An invalid host name is specified.");
                }

                try {
                    SslStream sslStream = new(_stream, false, sslConfiguration.ServerCertificateValidationCallback, sslConfiguration.ClientCertificateSelectionCallback);
                    sslStream.AuthenticateAsClient(targetHost, sslConfiguration.ClientCertificates, sslConfiguration.EnabledSslProtocols, sslConfiguration.CheckCertificateRevocation);
                    _stream = sslStream;
                } catch (Exception innerException) {
                    throw new WebSocketException(CloseStatusCode.TlsHandshakeFailure, innerException);
                }
            }
        }

        void startReceiving() {
            if (_messageEventQueue.Count > 0) {
                _messageEventQueue.Clear();
            }

            _exitReceiving = new AutoResetEvent(false);
            _receivePong = new AutoResetEvent(false);
            Action receive = null;

            receive = delegate {
                WebSocketFrame.ReadFrameAsync(_stream, false, delegate(WebSocketFrame frame) {
                    if (!processReceivedFrame(frame) || _readyState == WebSocketState.Closed) {
                        AutoResetEvent exitReceiving = _exitReceiving;

                        if (exitReceiving != null) {
                            exitReceiving.Set();
                        }
                    } else {
                        receive();

                        if (!_inMessage && HasMessage && _readyState == WebSocketState.Open) {
                            message();
                        }
                    }
                }, delegate(Exception ex) {
                    _logger.Fatal(ex.ToString());
                    fatal("An exception has occurred while receiving.", ex);
                });
            };

            receive();
        }

        bool validateSecWebSocketAcceptHeader(string value) => value != null && value == CreateResponseKey(_base64Key);

        bool validateSecWebSocketExtensionsClientHeader(string value) => value == null || value.Length > 0;

        bool validateSecWebSocketExtensionsServerHeader(string value) {
            if (value == null) {
                return true;
            }

            if (value.Length == 0) {
                return false;
            }

            if (!_extensionsRequested) {
                return false;
            }

            bool flag = _compression != CompressionMethod.None;

            foreach (string item in value.SplitHeaderValue(',')) {
                string text = item.Trim();

                if (flag && text.IsCompressionExtension(_compression)) {
                    if (!text.Contains("server_no_context_takeover")) {
                        _logger.Error("The server hasn't sent back 'server_no_context_takeover'.");
                        return false;
                    }

                    if (!text.Contains("client_no_context_takeover")) {
                        _logger.Warn("The server hasn't sent back 'client_no_context_takeover'.");
                    }

                    string method = _compression.ToExtensionString();

                    if (text.SplitHeaderValue(';').Contains(delegate(string t) {
                            t = t.Trim();
                            return t != method && t != "server_no_context_takeover" && t != "client_no_context_takeover";
                        })) {
                        return false;
                    }

                    continue;
                }

                return false;
            }

            return true;
        }

        bool validateSecWebSocketKeyHeader(string value) => value != null && value.Length > 0;

        bool validateSecWebSocketProtocolClientHeader(string value) => value == null || value.Length > 0;

        bool validateSecWebSocketProtocolServerHeader(string value) {
            if (value == null) {
                return !_protocolsRequested;
            }

            if (value.Length == 0) {
                return false;
            }

            return _protocolsRequested && _protocols.Contains(p => p == value);
        }

        bool validateSecWebSocketVersionClientHeader(string value) => value != null && value == "13";

        bool validateSecWebSocketVersionServerHeader(string value) => value == null || value == "13";

        internal static string CheckCloseParameters(ushort code, string reason, bool client) => !code.IsCloseStatusCode() ? "An invalid close status code." :
                                                                                                code == 1005 ? reason.IsNullOrEmpty() ? null : "NoStatus cannot have a reason." :
                                                                                                code == 1010 && !client ? "MandatoryExtension cannot be used by a server." :
                                                                                                code == 1011 && client ? "ServerError cannot be used by a client." :
                                                                                                reason.IsNullOrEmpty() || reason.UTF8Encode().Length <= 123 ? null :
                                                                                                "A reason has greater than the allowable max size.";

        internal static string CheckCloseParameters(CloseStatusCode code, string reason, bool client) => code == CloseStatusCode.NoStatus
                                                                                                             ? reason.IsNullOrEmpty() ? null : "NoStatus cannot have a reason."
                                                                                                             : code == CloseStatusCode.MandatoryExtension && !client
                                                                                                                 ? "MandatoryExtension cannot be used by a server."
                                                                                                                 : code == CloseStatusCode.ServerError && client
                                                                                                                     ? "ServerError cannot be used by a client."
                                                                                                                     : reason.IsNullOrEmpty() || reason.UTF8Encode().Length <= 123
                                                                                                                         ? null
                                                                                                                         : "A reason has greater than the allowable max size.";

        internal static string CheckPingParameter(string message, out byte[] bytes) {
            bytes = message.UTF8Encode();
            return bytes.Length <= 125 ? null : "A message has greater than the allowable max size.";
        }

        internal static string CheckSendParameter(byte[] data) => data != null ? null : "'data' is null.";

        internal static string CheckSendParameter(FileInfo file) => file != null ? null : "'file' is null.";

        internal static string CheckSendParameter(string data) => data != null ? null : "'data' is null.";

        internal static string CheckSendParameters(Stream stream, int length) =>
            stream == null ? "'stream' is null." : !stream.CanRead ? "'stream' cannot be read." : length >= 1 ? null : "'length' is less than 1.";

        internal void Close(HttpResponse response) {
            _readyState = WebSocketState.Closing;
            sendHttpResponse(response);
            releaseServerResources();
            _readyState = WebSocketState.Closed;
        }

        internal void Close(HttpStatusCode code) {
            Close(createHandshakeFailureResponse(code));
        }

        internal void Close(CloseEventArgs e, byte[] frameAsBytes, bool receive) {
            lock (_forConn) {
                if (_readyState == WebSocketState.Closing) {
                    _logger.Info("The closing is already in progress.");
                    return;
                }

                if (_readyState == WebSocketState.Closed) {
                    _logger.Info("The connection has been closed.");
                    return;
                }

                _readyState = WebSocketState.Closing;
            }

            e.WasClean = closeHandshake(frameAsBytes, receive, false);
            releaseServerResources();
            releaseCommonResources();
            _readyState = WebSocketState.Closed;

            try {
                OnClose.Emit(this, e);
            } catch (Exception ex) {
                _logger.Error(ex.ToString());
            }
        }

        internal static string CreateBase64Key() {
            byte[] array = new byte[16];
            RandomNumber.GetBytes(array);
            return Convert.ToBase64String(array);
        }

        internal static string CreateResponseKey(string base64Key) {
            StringBuilder stringBuilder = new(base64Key, 64);
            stringBuilder.Append("258EAFA5-E914-47DA-95CA-C5AB0DC85B11");
            SHA1 sHA = new SHA1CryptoServiceProvider();
            byte[] inArray = sHA.ComputeHash(stringBuilder.ToString().UTF8Encode());
            return Convert.ToBase64String(inArray);
        }

        internal void InternalAccept() {
            try {
                if (!acceptHandshake()) {
                    return;
                }

                _readyState = WebSocketState.Open;
            } catch (Exception ex) {
                _logger.Fatal(ex.ToString());
                fatal("An exception has occurred while accepting.", ex);
                return;
            }

            open();
        }

        internal bool Ping(byte[] frameAsBytes, TimeSpan timeout) {
            if (_readyState != WebSocketState.Open) {
                return false;
            }

            if (!send(frameAsBytes)) {
                return false;
            }

            AutoResetEvent receivePong = _receivePong;

            if (receivePong == null) {
                return false;
            }

            return receivePong.WaitOne(timeout);
        }

        internal void Send(Opcode opcode, byte[] data, Dictionary<CompressionMethod, byte[]> cache) {
            lock (_forSend) {
                lock (_forConn) {
                    if (_readyState != WebSocketState.Open) {
                        _logger.Error("The sending has been interrupted.");
                        return;
                    }

                    try {
                        byte[] value;

                        if (!cache.TryGetValue(_compression, out value)) {
                            value = new WebSocketFrame(Fin.Final, opcode, data.Compress(_compression), _compression != CompressionMethod.None, false).ToArray();
                            cache.Add(_compression, value);
                        }

                        sendBytes(value);
                    } catch (Exception ex) {
                        _logger.Error(ex.ToString());
                    }
                }
            }
        }

        internal void Send(Opcode opcode, Stream stream, Dictionary<CompressionMethod, Stream> cache) {
            lock (_forSend) {
                try {
                    Stream value;

                    if (!cache.TryGetValue(_compression, out value)) {
                        value = stream.Compress(_compression);
                        cache.Add(_compression, value);
                    } else {
                        value.Position = 0L;
                    }

                    send(opcode, value, _compression != CompressionMethod.None);
                } catch (Exception ex) {
                    _logger.Error(ex.ToString());
                }
            }
        }

        public void Accept() {
            string text;

            if (!checkIfAvailable(false, true, true, false, false, false, out text)) {
                _logger.Error(text);
                error("An error has occurred in accepting.", null);
            } else if (accept()) {
                open();
            }
        }

        public void AcceptAsync() {
            string text;

            if (!checkIfAvailable(false, true, true, false, false, false, out text)) {
                _logger.Error(text);
                error("An error has occurred in accepting.", null);
                return;
            }

            Func<bool> acceptor = accept;

            acceptor.BeginInvoke(delegate(IAsyncResult ar) {
                if (acceptor.EndInvoke(ar)) {
                    open();
                }
            }, null);
        }

        public void Close() {
            string text;

            if (!checkIfAvailable(true, true, false, false, out text)) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else {
                close(new CloseEventArgs(), true, true, false);
            }
        }

        public void Close(ushort code) {
            string text = _readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, null, _client);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else if (code == 1005) {
                close(new CloseEventArgs(), true, true, false);
            } else {
                bool receive = !code.IsReserved();
                close(new CloseEventArgs(code), receive, receive, false);
            }
        }

        public void Close(CloseStatusCode code) {
            string text = _readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, null, _client);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else if (code == CloseStatusCode.NoStatus) {
                close(new CloseEventArgs(), true, true, false);
            } else {
                bool receive = !code.IsReserved();
                close(new CloseEventArgs(code), receive, receive, false);
            }
        }

        public void Close(ushort code, string reason) {
            string text = _readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, reason, _client);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else if (code == 1005) {
                close(new CloseEventArgs(), true, true, false);
            } else {
                bool receive = !code.IsReserved();
                close(new CloseEventArgs(code, reason), receive, receive, false);
            }
        }

        public void Close(CloseStatusCode code, string reason) {
            string text = _readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, reason, _client);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else if (code == CloseStatusCode.NoStatus) {
                close(new CloseEventArgs(), true, true, false);
            } else {
                bool receive = !code.IsReserved();
                close(new CloseEventArgs(code, reason), receive, receive, false);
            }
        }

        public void CloseAsync() {
            string text;

            if (!checkIfAvailable(true, true, false, false, out text)) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else {
                closeAsync(new CloseEventArgs(), true, true, false);
            }
        }

        public void CloseAsync(ushort code) {
            string text = _readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, null, _client);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else if (code == 1005) {
                closeAsync(new CloseEventArgs(), true, true, false);
            } else {
                bool receive = !code.IsReserved();
                closeAsync(new CloseEventArgs(code), receive, receive, false);
            }
        }

        public void CloseAsync(CloseStatusCode code) {
            string text = _readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, null, _client);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else if (code == CloseStatusCode.NoStatus) {
                closeAsync(new CloseEventArgs(), true, true, false);
            } else {
                bool receive = !code.IsReserved();
                closeAsync(new CloseEventArgs(code), receive, receive, false);
            }
        }

        public void CloseAsync(ushort code, string reason) {
            string text = _readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, reason, _client);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else if (code == 1005) {
                closeAsync(new CloseEventArgs(), true, true, false);
            } else {
                bool receive = !code.IsReserved();
                closeAsync(new CloseEventArgs(code, reason), receive, receive, false);
            }
        }

        public void CloseAsync(CloseStatusCode code, string reason) {
            string text = _readyState.CheckIfAvailable(true, true, false, false) ?? CheckCloseParameters(code, reason, _client);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in closing the connection.", null);
            } else if (code == CloseStatusCode.NoStatus) {
                closeAsync(new CloseEventArgs(), true, true, false);
            } else {
                bool receive = !code.IsReserved();
                closeAsync(new CloseEventArgs(code, reason), receive, receive, false);
            }
        }

        public void Connect() {
            string text;

            if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                _logger.Error(text);
                error("An error has occurred in connecting.", null);
            } else if (connect()) {
                open();
            }
        }

        public void ConnectAsync() {
            string text;

            if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                _logger.Error(text);
                error("An error has occurred in connecting.", null);
                return;
            }

            Func<bool> connector = connect;

            connector.BeginInvoke(delegate(IAsyncResult ar) {
                if (connector.EndInvoke(ar)) {
                    open();
                }
            }, null);
        }

        public bool Ping() {
            byte[] frameAsBytes = !_client ? WebSocketFrame.EmptyPingBytes : WebSocketFrame.CreatePingFrame(true).ToArray();
            return Ping(frameAsBytes, _waitTime);
        }

        public bool Ping(string message) {
            if (message == null || message.Length == 0) {
                return Ping();
            }

            byte[] bytes;
            string text = CheckPingParameter(message, out bytes);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in sending a ping.", null);
                return false;
            }

            return Ping(WebSocketFrame.CreatePingFrame(bytes, _client).ToArray(), _waitTime);
        }

        public void Send(byte[] data) {
            string text = _readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(data);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in sending data.", null);
            } else {
                send(Opcode.Binary, new MemoryStream(data));
            }
        }

        public void Send(FileInfo file) {
            string text = _readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(file);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in sending data.", null);
            } else {
                send(Opcode.Binary, file.OpenRead());
            }
        }

        public void Send(string data) {
            string text = _readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(data);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in sending data.", null);
            } else {
                send(Opcode.Text, new MemoryStream(data.UTF8Encode()));
            }
        }

        public void SendAsync(byte[] data, Action<bool> completed) {
            string text = _readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(data);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in sending data.", null);
            } else {
                sendAsync(Opcode.Binary, new MemoryStream(data), completed);
            }
        }

        public void SendAsync(FileInfo file, Action<bool> completed) {
            string text = _readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(file);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in sending data.", null);
            } else {
                sendAsync(Opcode.Binary, file.OpenRead(), completed);
            }
        }

        public void SendAsync(string data, Action<bool> completed) {
            string text = _readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameter(data);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in sending data.", null);
            } else {
                sendAsync(Opcode.Text, new MemoryStream(data.UTF8Encode()), completed);
            }
        }

        public void SendAsync(Stream stream, int length, Action<bool> completed) {
            string text = _readyState.CheckIfAvailable(false, true, false, false) ?? CheckSendParameters(stream, length);

            if (text != null) {
                _logger.Error(text);
                error("An error has occurred in sending data.", null);
                return;
            }

            stream.ReadBytesAsync(length, delegate(byte[] data) {
                int num = data.Length;

                if (num == 0) {
                    _logger.Error("The data cannot be read from 'stream'.");
                    error("An error has occurred in sending data.", null);
                } else {
                    if (num < length) {
                        _logger.Warn(string.Format("The length of the data is less than 'length':\n  expected: {0}\n  actual: {1}", length, num));
                    }

                    bool obj = send(Opcode.Binary, new MemoryStream(data));

                    if (completed != null) {
                        completed(obj);
                    }
                }
            }, delegate(Exception ex) {
                _logger.Error(ex.ToString());
                error("An exception has occurred while sending data.", ex);
            });
        }

        public void SetCookie(Cookie cookie) {
            string text;

            if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                _logger.Error(text);
                error("An error has occurred in setting a cookie.", null);
                return;
            }

            lock (_forConn) {
                if (!checkIfAvailable(true, false, false, true, out text)) {
                    _logger.Error(text);
                    error("An error has occurred in setting a cookie.", null);
                    return;
                }

                if (cookie == null) {
                    _logger.Error("'cookie' is null.");
                    error("An error has occurred in setting a cookie.", null);
                    return;
                }

                lock (CookieCollection.SyncRoot) {
                    CookieCollection.SetOrRemove(cookie);
                }
            }
        }

        public void SetCredentials(string username, string password, bool preAuth) {
            string text;

            if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                _logger.Error(text);
                error("An error has occurred in setting the credentials.", null);
                return;
            }

            lock (_forConn) {
                if (!checkIfAvailable(true, false, false, true, out text)) {
                    _logger.Error(text);
                    error("An error has occurred in setting the credentials.", null);
                } else if (username.IsNullOrEmpty()) {
                    _logger.Warn("The credentials are set back to the default.");
                    Credentials = null;
                    _preAuth = false;
                } else if (username.Contains(':') || !username.IsText()) {
                    _logger.Error("'username' contains an invalid character.");
                    error("An error has occurred in setting the credentials.", null);
                } else if (!password.IsNullOrEmpty() && !password.IsText()) {
                    _logger.Error("'password' contains an invalid character.");
                    error("An error has occurred in setting the credentials.", null);
                } else {
                    Credentials = new NetworkCredential(username, password, _uri.PathAndQuery);
                    _preAuth = preAuth;
                }
            }
        }

        public void SetProxy(string url, string username, string password) {
            string text;

            if (!checkIfAvailable(true, false, true, false, false, true, out text)) {
                _logger.Error(text);
                error("An error has occurred in setting the proxy.", null);
                return;
            }

            lock (_forConn) {
                Uri result;

                if (!checkIfAvailable(true, false, false, true, out text)) {
                    _logger.Error(text);
                    error("An error has occurred in setting the proxy.", null);
                } else if (url.IsNullOrEmpty()) {
                    _logger.Warn("The proxy url and credentials are set back to the default.");
                    _proxyUri = null;
                    _proxyCredentials = null;
                } else if (!Uri.TryCreate(url, UriKind.Absolute, out result) || result.Scheme != "http" || result.Segments.Length > 1) {
                    _logger.Error("The syntax of a proxy url must be 'http://<host>[:<port>]'.");
                    error("An error has occurred in setting the proxy.", null);
                } else if (username.IsNullOrEmpty()) {
                    _logger.Warn("The proxy credentials are set back to the default.");
                    _proxyUri = result;
                    _proxyCredentials = null;
                } else if (username.Contains(':') || !username.IsText()) {
                    _logger.Error("'username' contains an invalid character.");
                    error("An error has occurred in setting the proxy.", null);
                } else if (!password.IsNullOrEmpty() && !password.IsText()) {
                    _logger.Error("'password' contains an invalid character.");
                    error("An error has occurred in setting the proxy.", null);
                } else {
                    _proxyUri = result;
                    _proxyCredentials = new NetworkCredential(username, password, string.Format("{0}:{1}", _uri.DnsSafeHost, _uri.Port));
                }
            }
        }
    }
}