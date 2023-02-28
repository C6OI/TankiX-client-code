using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace WebSocketSharp.Net {
    public sealed class HttpListenerResponse : IDisposable {
        Encoding _contentEncoding;

        long _contentLength;

        string _contentType;

        readonly HttpListenerContext _context;

        CookieCollection _cookies;

        bool _disposed;

        WebHeaderCollection _headers;

        bool _keepAlive;

        string _location;

        ResponseStream _outputStream;

        bool _sendChunked;

        int _statusCode;

        string _statusDescription;

        Version _version;

        internal HttpListenerResponse(HttpListenerContext context) {
            _context = context;
            _keepAlive = true;
            _statusCode = 200;
            _statusDescription = "OK";
            _version = HttpVersion.Version11;
        }

        internal bool CloseConnection { get; set; }

        internal bool HeadersSent { get; set; }

        public Encoding ContentEncoding {
            get => _contentEncoding;
            set {
                checkDisposed();
                _contentEncoding = value;
            }
        }

        public long ContentLength64 {
            get => _contentLength;
            set {
                checkDisposedOrHeadersSent();

                if (value < 0) {
                    throw new ArgumentOutOfRangeException("Less than zero.", "value");
                }

                _contentLength = value;
            }
        }

        public string ContentType {
            get => _contentType;
            set {
                checkDisposed();

                if (value != null && value.Length == 0) {
                    throw new ArgumentException("An empty string.", "value");
                }

                _contentType = value;
            }
        }

        public CookieCollection Cookies {
            get => _cookies ?? (_cookies = new CookieCollection());
            set => _cookies = value;
        }

        public WebHeaderCollection Headers {
            get => _headers ?? (_headers = new WebHeaderCollection(HttpHeaderType.Response, false));
            set {
                if (value != null && value.State != HttpHeaderType.Response) {
                    throw new InvalidOperationException("The specified headers aren't valid for a response.");
                }

                _headers = value;
            }
        }

        public bool KeepAlive {
            get => _keepAlive;
            set {
                checkDisposedOrHeadersSent();
                _keepAlive = value;
            }
        }

        public Stream OutputStream {
            get {
                checkDisposed();
                return _outputStream ?? (_outputStream = _context.Connection.GetResponseStream());
            }
        }

        public Version ProtocolVersion {
            get => _version;
            set {
                checkDisposedOrHeadersSent();

                if (value == null) {
                    throw new ArgumentNullException("value");
                }

                if (value.Major != 1 || value.Minor != 0 && value.Minor != 1) {
                    throw new ArgumentException("Not 1.0 or 1.1.", "value");
                }

                _version = value;
            }
        }

        public string RedirectLocation {
            get => _location;
            set {
                checkDisposed();

                if (value == null) {
                    _location = null;
                    return;
                }

                Uri result = null;

                if (!value.MaybeUri() || !Uri.TryCreate(value, UriKind.Absolute, out result)) {
                    throw new ArgumentException("Not an absolute URL.", "value");
                }

                _location = value;
            }
        }

        public bool SendChunked {
            get => _sendChunked;
            set {
                checkDisposedOrHeadersSent();
                _sendChunked = value;
            }
        }

        public int StatusCode {
            get => _statusCode;
            set {
                checkDisposedOrHeadersSent();

                if (value < 100 || value > 999) {
                    throw new ProtocolViolationException("A value isn't between 100 and 999 inclusive.");
                }

                _statusCode = value;
                _statusDescription = value.GetStatusDescription();
            }
        }

        public string StatusDescription {
            get => _statusDescription;
            set {
                checkDisposedOrHeadersSent();

                if (value == null || value.Length == 0) {
                    _statusDescription = _statusCode.GetStatusDescription();
                    return;
                }

                if (!value.IsText() || value.IndexOfAny(new char[2] { '\r', '\n' }) > -1) {
                    throw new ArgumentException("Contains invalid characters.", "value");
                }

                _statusDescription = value;
            }
        }

        void IDisposable.Dispose() {
            if (!_disposed) {
                close(true);
            }
        }

        bool canAddOrUpdate(Cookie cookie) {
            if (_cookies == null || _cookies.Count == 0) {
                return true;
            }

            List<Cookie> list = findCookie(cookie).ToList();

            if (list.Count == 0) {
                return true;
            }

            int version = cookie.Version;

            foreach (Cookie item in list) {
                if (item.Version == version) {
                    return true;
                }
            }

            return false;
        }

        void checkDisposed() {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }
        }

        void checkDisposedOrHeadersSent() {
            if (_disposed) {
                throw new ObjectDisposedException(GetType().ToString());
            }

            if (HeadersSent) {
                throw new InvalidOperationException("Cannot be changed after the headers are sent.");
            }
        }

        void close(bool force) {
            _disposed = true;
            _context.Connection.Close(force);
        }

        IEnumerable<Cookie> findCookie(Cookie cookie) {
            string name = cookie.Name;
            string domain = cookie.Domain;
            string path = cookie.Path;

            if (_cookies == null) {
                yield break;
            }

            IEnumerator enumerator = _cookies.GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    Cookie c = (Cookie)enumerator.Current;

                    if (c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Domain.Equals(domain, StringComparison.OrdinalIgnoreCase) &&
                        c.Path.Equals(path, StringComparison.Ordinal)) {
                        yield return c;
                    }
                }
            } finally {
                IDisposable disposable;
                IDisposable disposable2 = disposable = enumerator as IDisposable;

                if (disposable != null) {
                    disposable2.Dispose();
                }
            }
        }

        internal WebHeaderCollection WriteHeadersTo(MemoryStream destination) {
            WebHeaderCollection webHeaderCollection = new(HttpHeaderType.Response, true);

            if (_headers != null) {
                webHeaderCollection.Add(_headers);
            }

            if (_contentType != null) {
                string value = _contentType.IndexOf("charset=", StringComparison.Ordinal) != -1 || _contentEncoding == null ? _contentType
                                   : string.Format("{0}; charset={1}", _contentType, _contentEncoding.WebName);

                webHeaderCollection.InternalSet("Content-Type", value, true);
            }

            if (webHeaderCollection["Server"] == null) {
                webHeaderCollection.InternalSet("Server", "websocket-sharp/1.0", true);
            }

            CultureInfo invariantCulture = CultureInfo.InvariantCulture;

            if (webHeaderCollection["Date"] == null) {
                webHeaderCollection.InternalSet("Date", DateTime.UtcNow.ToString("r", invariantCulture), true);
            }

            if (!_sendChunked) {
                webHeaderCollection.InternalSet("Content-Length", _contentLength.ToString(invariantCulture), true);
            } else {
                webHeaderCollection.InternalSet("Transfer-Encoding", "chunked", true);
            }

            bool flag = !_context.Request.KeepAlive || !_keepAlive || _statusCode == 400 || _statusCode == 408 || _statusCode == 411 || _statusCode == 413 || _statusCode == 414 ||
                        _statusCode == 500 || _statusCode == 503;

            int reuses = _context.Connection.Reuses;

            if (flag || reuses >= 100) {
                webHeaderCollection.InternalSet("Connection", "close", true);
            } else {
                webHeaderCollection.InternalSet("Keep-Alive", string.Format("timeout=15,max={0}", 100 - reuses), true);

                if (_context.Request.ProtocolVersion < HttpVersion.Version11) {
                    webHeaderCollection.InternalSet("Connection", "keep-alive", true);
                }
            }

            if (_location != null) {
                webHeaderCollection.InternalSet("Location", _location, true);
            }

            if (_cookies != null) {
                IEnumerator enumerator = _cookies.GetEnumerator();

                try {
                    while (enumerator.MoveNext()) {
                        Cookie cookie = (Cookie)enumerator.Current;
                        webHeaderCollection.InternalSet("Set-Cookie", cookie.ToResponseString(), true);
                    }
                } finally {
                    IDisposable disposable;

                    if ((disposable = enumerator as IDisposable) != null) {
                        disposable.Dispose();
                    }
                }
            }

            Encoding encoding = _contentEncoding ?? Encoding.Default;
            StreamWriter streamWriter = new(destination, encoding, 256);
            streamWriter.Write("HTTP/{0} {1} {2}\r\n", _version, _statusCode, _statusDescription);
            streamWriter.Write(webHeaderCollection.ToStringMultiValue(true));
            streamWriter.Flush();
            destination.Position = encoding.GetPreamble().Length;
            return webHeaderCollection;
        }

        public void Abort() {
            if (!_disposed) {
                close(true);
            }
        }

        public void AddHeader(string name, string value) {
            Headers.Set(name, value);
        }

        public void AppendCookie(Cookie cookie) {
            Cookies.Add(cookie);
        }

        public void AppendHeader(string name, string value) {
            Headers.Add(name, value);
        }

        public void Close() {
            if (!_disposed) {
                close(false);
            }
        }

        public void Close(byte[] responseEntity, bool willBlock) {
            checkDisposed();

            if (responseEntity == null) {
                throw new ArgumentNullException("responseEntity");
            }

            int count = responseEntity.Length;
            Stream output = OutputStream;

            if (willBlock) {
                output.Write(responseEntity, 0, count);
                close(false);
                return;
            }

            output.BeginWrite(responseEntity, 0, count, delegate(IAsyncResult ar) {
                output.EndWrite(ar);
                close(false);
            }, null);
        }

        public void CopyFrom(HttpListenerResponse templateResponse) {
            if (templateResponse == null) {
                throw new ArgumentNullException("templateResponse");
            }

            if (templateResponse._headers != null) {
                if (_headers != null) {
                    _headers.Clear();
                }

                Headers.Add(templateResponse._headers);
            } else if (_headers != null) {
                _headers = null;
            }

            _contentLength = templateResponse._contentLength;
            _statusCode = templateResponse._statusCode;
            _statusDescription = templateResponse._statusDescription;
            _keepAlive = templateResponse._keepAlive;
            _version = templateResponse._version;
        }

        public void Redirect(string url) {
            checkDisposedOrHeadersSent();

            if (url == null) {
                throw new ArgumentNullException("url");
            }

            Uri result = null;

            if (!url.MaybeUri() || !Uri.TryCreate(url, UriKind.Absolute, out result)) {
                throw new ArgumentException("Not an absolute URL.", "url");
            }

            _location = url;
            _statusCode = 302;
            _statusDescription = "Found";
        }

        public void SetCookie(Cookie cookie) {
            if (cookie == null) {
                throw new ArgumentNullException("cookie");
            }

            if (!canAddOrUpdate(cookie)) {
                throw new ArgumentException("Cannot be replaced.", "cookie");
            }

            Cookies.Add(cookie);
        }
    }
}