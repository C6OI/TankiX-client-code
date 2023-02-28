using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WebSocketSharp.Net {
    public sealed class HttpListenerRequest {
        static readonly byte[] _100continue;

        bool _chunked;

        Encoding _contentEncoding;

        bool _contentLengthSet;

        readonly HttpListenerContext _context;

        CookieCollection _cookies;

        readonly WebHeaderCollection _headers;

        Stream _inputStream;

        bool _keepAlive;

        bool _keepAliveSet;

        NameValueCollection _queryString;

        string _uri;

        Version _version;

        bool _websocketRequest;

        bool _websocketRequestSet;

        static HttpListenerRequest() => _100continue = Encoding.ASCII.GetBytes("HTTP/1.1 100 Continue\r\n\r\n");

        internal HttpListenerRequest(HttpListenerContext context) {
            _context = context;
            ContentLength64 = -1L;
            _headers = new WebHeaderCollection();
            RequestTraceIdentifier = Guid.NewGuid();
        }

        public string[] AcceptTypes { get; private set; }

        public int ClientCertificateError => 0;

        public Encoding ContentEncoding => _contentEncoding ?? (_contentEncoding = Encoding.Default);

        public long ContentLength64 { get; private set; }

        public string ContentType => _headers["Content-Type"];

        public CookieCollection Cookies => _cookies ?? (_cookies = _headers.GetCookies(false));

        public bool HasEntityBody => ContentLength64 > 0 || _chunked;

        public NameValueCollection Headers => _headers;

        public string HttpMethod { get; private set; }

        public Stream InputStream => _inputStream ?? (_inputStream = !HasEntityBody ? Stream.Null : _context.Connection.GetRequestStream(ContentLength64, _chunked));

        public bool IsAuthenticated => _context.User != null;

        public bool IsLocal => RemoteEndPoint.Address.IsLocal();

        public bool IsSecureConnection => _context.Connection.IsSecure;

        public bool IsWebSocketRequest {
            get {
                if (!_websocketRequestSet) {
                    _websocketRequest = HttpMethod == "GET" && _version > HttpVersion.Version10 && _headers.Contains("Upgrade", "websocket") && _headers.Contains("Connection", "Upgrade");
                    _websocketRequestSet = true;
                }

                return _websocketRequest;
            }
        }

        public bool KeepAlive {
            get {
                if (!_keepAliveSet) {
                    string text;
                    _keepAlive = _version > HttpVersion.Version10 || _headers.Contains("Connection", "keep-alive") || (text = _headers["Keep-Alive"]) != null && text != "closed";
                    _keepAliveSet = true;
                }

                return _keepAlive;
            }
        }

        public IPEndPoint LocalEndPoint => _context.Connection.LocalEndPoint;

        public Version ProtocolVersion => _version;

        public NameValueCollection QueryString => _queryString ?? (_queryString = HttpUtility.InternalParseQueryString(Url.Query, Encoding.UTF8));

        public string RawUrl => Url.PathAndQuery;

        public IPEndPoint RemoteEndPoint => _context.Connection.RemoteEndPoint;

        public Guid RequestTraceIdentifier { get; }

        public Uri Url { get; private set; }

        public Uri UrlReferrer { get; private set; }

        public string UserAgent => _headers["User-Agent"];

        public string UserHostAddress => LocalEndPoint.ToString();

        public string UserHostName => _headers["Host"];

        public string[] UserLanguages { get; private set; }

        static bool tryCreateVersion(string version, out Version result) {
            try {
                result = new Version(version);
                return true;
            } catch {
                result = null;
                return false;
            }
        }

        internal void AddHeader(string header) {
            int num = header.IndexOf(':');

            if (num == -1) {
                _context.ErrorMessage = "Invalid header";
                return;
            }

            string text = header.Substring(0, num).Trim();
            string text2 = header.Substring(num + 1).Trim();
            _headers.InternalSet(text, text2, false);

            switch (text.ToLower(CultureInfo.InvariantCulture)) {
                case "accept":
                    AcceptTypes = new List<string>(text2.SplitHeaderValue(',')).ToArray();
                    break;

                case "accept-language":
                    UserLanguages = text2.Split(',');
                    break;

                case "content-length": {
                    long result;

                    if (long.TryParse(text2, out result) && result >= 0) {
                        ContentLength64 = result;
                        _contentLengthSet = true;
                    } else {
                        _context.ErrorMessage = "Invalid Content-Length header";
                    }

                    break;
                }

                case "content-type":
                    try {
                        _contentEncoding = HttpUtility.GetEncoding(text2);
                        break;
                    } catch {
                        _context.ErrorMessage = "Invalid Content-Type header";
                        break;
                    }

                case "referer":
                    UrlReferrer = text2.ToUri();
                    break;
            }
        }

        internal void FinishInitialization() {
            string text = _headers["Host"];
            bool flag = text == null || text.Length == 0;

            if (_version > HttpVersion.Version10 && flag) {
                _context.ErrorMessage = "Invalid Host header";
                return;
            }

            if (flag) {
                text = UserHostAddress;
            }

            Url = HttpUtility.CreateRequestUrl(_uri, text, IsWebSocketRequest, IsSecureConnection);

            if (Url == null) {
                _context.ErrorMessage = "Invalid request url";
                return;
            }

            string text2 = Headers["Transfer-Encoding"];

            if (_version > HttpVersion.Version10 && text2 != null && text2.Length > 0) {
                _chunked = text2.ToLower() == "chunked";

                if (!_chunked) {
                    _context.ErrorMessage = string.Empty;
                    _context.ErrorStatus = 501;
                    return;
                }
            }

            if (!_chunked && !_contentLengthSet) {
                string text3 = HttpMethod.ToLower();

                if (text3 == "post" || text3 == "put") {
                    _context.ErrorMessage = string.Empty;
                    _context.ErrorStatus = 411;
                    return;
                }
            }

            string text4 = Headers["Expect"];

            if (text4 != null && text4.Length > 0 && text4.ToLower() == "100-continue") {
                ResponseStream responseStream = _context.Connection.GetResponseStream();
                responseStream.InternalWrite(_100continue, 0, _100continue.Length);
            }
        }

        internal bool FlushInput() {
            if (!HasEntityBody) {
                return true;
            }

            int num = 2048;

            if (ContentLength64 > 0) {
                num = (int)Math.Min(ContentLength64, num);
            }

            byte[] buffer = new byte[num];

            while (true) {
                try {
                    IAsyncResult asyncResult = InputStream.BeginRead(buffer, 0, num, null, null);

                    if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(100)) {
                        return false;
                    }

                    if (InputStream.EndRead(asyncResult) <= 0) {
                        return true;
                    }
                } catch {
                    return false;
                }
            }
        }

        internal void SetRequestLine(string requestLine) {
            string[] array = requestLine.Split(new char[1] { ' ' }, 3);

            if (array.Length != 3) {
                _context.ErrorMessage = "Invalid request line (parts)";
                return;
            }

            HttpMethod = array[0];

            if (!HttpMethod.IsToken()) {
                _context.ErrorMessage = "Invalid request line (method)";
                return;
            }

            _uri = array[1];
            string text = array[2];

            if (text.Length != 8 || !text.StartsWith("HTTP/") || !tryCreateVersion(text.Substring(5), out _version) || _version.Major < 1) {
                _context.ErrorMessage = "Invalid request line (version)";
            }
        }

        public IAsyncResult BeginGetClientCertificate(AsyncCallback requestCallback, object state) => throw new NotImplementedException();

        public X509Certificate2 EndGetClientCertificate(IAsyncResult asyncResult) => throw new NotImplementedException();

        public X509Certificate2 GetClientCertificate() => throw new NotImplementedException();

        public override string ToString() {
            StringBuilder stringBuilder = new(64);
            stringBuilder.AppendFormat("{0} {1} HTTP/{2}\r\n", HttpMethod, _uri, _version);
            stringBuilder.Append(_headers);
            return stringBuilder.ToString();
        }
    }
}