using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp.Server;
using CookieCollection = WebSocketSharp.Net.CookieCollection;
using HttpListenerRequest = WebSocketSharp.Net.HttpListenerRequest;
using HttpListenerResponse = WebSocketSharp.Net.HttpListenerResponse;
using HttpStatusCode = WebSocketSharp.Net.HttpStatusCode;

namespace WebSocketSharp {
    public static class Ext {
        const string _tspecials = "()<>@,;:\\\"/[]?={} \t";
        static readonly byte[] _last = new byte[1];

        static byte[] compress(this byte[] data) {
            if (data.LongLength == 0) {
                return data;
            }

            using (MemoryStream stream = new(data)) {
                return stream.compressToArray();
            }
        }

        static MemoryStream compress(this Stream stream) {
            MemoryStream memoryStream = new();

            if (stream.Length == 0) {
                return memoryStream;
            }

            stream.Position = 0L;

            using (DeflateStream deflateStream = new(memoryStream, CompressionMode.Compress, true)) {
                stream.CopyTo(deflateStream, 1024);
                deflateStream.Close();
                memoryStream.Write(_last, 0, 1);
                memoryStream.Position = 0L;
                return memoryStream;
            }
        }

        static byte[] compressToArray(this Stream stream) {
            using (MemoryStream memoryStream = stream.compress()) {
                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }

        static byte[] decompress(this byte[] data) {
            if (data.LongLength == 0) {
                return data;
            }

            using (MemoryStream stream = new(data)) {
                return stream.decompressToArray();
            }
        }

        static MemoryStream decompress(this Stream stream) {
            MemoryStream memoryStream = new();

            if (stream.Length == 0) {
                return memoryStream;
            }

            stream.Position = 0L;

            using (DeflateStream source = new(stream, CompressionMode.Decompress, true)) {
                source.CopyTo(memoryStream, 1024);
                memoryStream.Position = 0L;
                return memoryStream;
            }
        }

        static byte[] decompressToArray(this Stream stream) {
            using (MemoryStream memoryStream = stream.decompress()) {
                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }

        static void times(this ulong n, Action action) {
            for (ulong num = 0uL; num < n; num++) {
                action();
            }
        }

        internal static byte[] Append(this ushort code, string reason) {
            byte[] array = code.InternalToByteArray(ByteOrder.Big);

            if (reason != null && reason.Length > 0) {
                List<byte> list = new(array);
                list.AddRange(Encoding.UTF8.GetBytes(reason));
                array = list.ToArray();
            }

            return array;
        }

        internal static string CheckIfAvailable(this ServerState state, bool ready, bool start, bool shutting) =>
            (ready || state != 0 && state != ServerState.Stop) && (start || state != ServerState.Start) && (shutting || state != ServerState.ShuttingDown) ? null
                : "This operation isn't available in: " + state.ToString().ToLower();

        internal static string CheckIfAvailable(this WebSocketState state, bool connecting, bool open, bool closing, bool closed) =>
            (connecting || state != 0) && (open || state != WebSocketState.Open) && (closing || state != WebSocketState.Closing) && (closed || state != WebSocketState.Closed) ? null
                : "This operation isn't available in: " + state.ToString().ToLower();

        internal static string CheckIfValidProtocols(this string[] protocols) {
            return protocols.Contains(protocol => protocol == null || protocol.Length == 0 || !protocol.IsToken()) ? "Contains an invalid value." :
                   !protocols.ContainsTwice() ? null : "Contains a value twice.";
        }

        internal static string CheckIfValidServicePath(this string path) {
            return path == null || path.Length == 0 ? "'path' is null or empty." :
                   path[0] != '/' ? "'path' isn't an absolute path." :
                   path.IndexOfAny(new char[2] { '?', '#' }) <= -1 ? null : "'path' includes either or both query and fragment components.";
        }

        internal static string CheckIfValidSessionID(this string id) => id != null && id.Length != 0 ? null : "'id' is null or empty.";

        internal static string CheckIfValidWaitTime(this TimeSpan time) => !(time <= TimeSpan.Zero) ? null : "A wait time is zero or less.";

        internal static bool CheckWaitTime(this TimeSpan time, out string message) {
            message = null;

            if (time <= TimeSpan.Zero) {
                message = "A wait time is zero or less.";
                return false;
            }

            return true;
        }

        internal static void Close(this HttpListenerResponse response, HttpStatusCode code) {
            response.StatusCode = (int)code;
            response.OutputStream.Close();
        }

        internal static void CloseWithAuthChallenge(this HttpListenerResponse response, string challenge) {
            response.Headers.InternalSet("WWW-Authenticate", challenge, true);
            response.Close(HttpStatusCode.Unauthorized);
        }

        internal static byte[] Compress(this byte[] data, CompressionMethod method) => method != CompressionMethod.Deflate ? data : data.compress();

        internal static Stream Compress(this Stream stream, CompressionMethod method) => method != CompressionMethod.Deflate ? stream : stream.compress();

        internal static byte[] CompressToArray(this Stream stream, CompressionMethod method) => method != CompressionMethod.Deflate ? stream.ToByteArray() : stream.compressToArray();

        internal static bool Contains<T>(this IEnumerable<T> source, Func<T, bool> condition) {
            foreach (T item in source) {
                if (condition(item)) {
                    return true;
                }
            }

            return false;
        }

        internal static bool ContainsTwice(this string[] values) {
            int len = values.Length;
            Func<int, bool> contains = null;

            contains = delegate(int idx) {
                if (idx < len - 1) {
                    for (int i = idx + 1; i < len; i++) {
                        if (values[i] == values[idx]) {
                            return true;
                        }
                    }

                    return contains(++idx);
                }

                return false;
            };

            return contains(0);
        }

        internal static T[] Copy<T>(this T[] source, long length) {
            T[] array = new T[length];
            Array.Copy(source, 0L, array, 0L, length);
            return array;
        }

        internal static void CopyTo(this Stream source, Stream destination, int bufferLength) {
            byte[] buffer = new byte[bufferLength];
            int num = 0;

            while ((num = source.Read(buffer, 0, bufferLength)) > 0) {
                destination.Write(buffer, 0, num);
            }
        }

        internal static void CopyToAsync(this Stream source, Stream destination, int bufferLength, Action completed, Action<Exception> error) {
            byte[] buff = new byte[bufferLength];
            AsyncCallback callback = null;

            callback = delegate(IAsyncResult ar) {
                try {
                    int num = source.EndRead(ar);

                    if (num <= 0) {
                        if (completed != null) {
                            completed();
                        }
                    } else {
                        destination.Write(buff, 0, num);
                        source.BeginRead(buff, 0, bufferLength, callback, null);
                    }
                } catch (Exception obj2) {
                    if (error != null) {
                        error(obj2);
                    }
                }
            };

            try {
                source.BeginRead(buff, 0, bufferLength, callback, null);
            } catch (Exception obj) {
                if (error != null) {
                    error(obj);
                }
            }
        }

        internal static byte[] Decompress(this byte[] data, CompressionMethod method) => method != CompressionMethod.Deflate ? data : data.decompress();

        internal static Stream Decompress(this Stream stream, CompressionMethod method) => method != CompressionMethod.Deflate ? stream : stream.decompress();

        internal static byte[] DecompressToArray(this Stream stream, CompressionMethod method) => method != CompressionMethod.Deflate ? stream.ToByteArray() : stream.decompressToArray();

        internal static bool EqualsWith(this int value, char c, Action<int> action) {
            action(value);
            return value == c;
        }

        internal static string GetAbsolutePath(this Uri uri) {
            if (uri.IsAbsoluteUri) {
                return uri.AbsolutePath;
            }

            string originalString = uri.OriginalString;

            if (originalString[0] != '/') {
                return null;
            }

            int num = originalString.IndexOfAny(new char[2] { '?', '#' });
            return num <= 0 ? originalString : originalString.Substring(0, num);
        }

        internal static string GetMessage(this CloseStatusCode code) {
            object result;

            switch (code) {
                case CloseStatusCode.ProtocolError:
                    result = "A WebSocket protocol error has occurred.";
                    break;

                case CloseStatusCode.UnsupportedData:
                    result = "Unsupported data has been received.";
                    break;

                case CloseStatusCode.Abnormal:
                    result = "An exception has occurred.";
                    break;

                case CloseStatusCode.InvalidData:
                    result = "Invalid data has been received.";
                    break;

                case CloseStatusCode.PolicyViolation:
                    result = "A policy violation has occurred.";
                    break;

                case CloseStatusCode.TooBig:
                    result = "A too big message has been received.";
                    break;

                case CloseStatusCode.MandatoryExtension:
                    result = "WebSocket client didn't receive expected extension(s).";
                    break;

                case CloseStatusCode.ServerError:
                    result = "WebSocket server got an internal error.";
                    break;

                case CloseStatusCode.TlsHandshakeFailure:
                    result = "An error has occurred during a TLS handshake.";
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            return (string)result;
        }

        internal static string GetName(this string nameAndValue, char separator) {
            int num = nameAndValue.IndexOf(separator);
            return num <= 0 ? null : nameAndValue.Substring(0, num).Trim();
        }

        internal static string GetValue(this string nameAndValue, char separator) {
            int num = nameAndValue.IndexOf(separator);
            return num <= -1 || num >= nameAndValue.Length - 1 ? null : nameAndValue.Substring(num + 1).Trim();
        }

        internal static string GetValue(this string nameAndValue, char separator, bool unquote) {
            int num = nameAndValue.IndexOf(separator);

            if (num < 0 || num == nameAndValue.Length - 1) {
                return null;
            }

            string text = nameAndValue.Substring(num + 1).Trim();
            return !unquote ? text : text.Unquote();
        }

        internal static TcpListenerWebSocketContext GetWebSocketContext(this TcpClient tcpClient, string protocol, bool secure, ServerSslConfiguration sslConfig, Logger logger) =>
            new(tcpClient, protocol, secure, sslConfig, logger);

        internal static byte[] InternalToByteArray(this ushort value, ByteOrder order) {
            byte[] bytes = BitConverter.GetBytes(value);

            if (!order.IsHostOrder()) {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        internal static byte[] InternalToByteArray(this ulong value, ByteOrder order) {
            byte[] bytes = BitConverter.GetBytes(value);

            if (!order.IsHostOrder()) {
                Array.Reverse(bytes);
            }

            return bytes;
        }

        internal static bool IsCompressionExtension(this string value, CompressionMethod method) => value.StartsWith(method.ToExtensionString());

        internal static bool IsControl(this byte opcode) => opcode > 7 && opcode < 16;

        internal static bool IsControl(this Opcode opcode) => (int)opcode >= 8;

        internal static bool IsData(this byte opcode) => opcode == 1 || opcode == 2;

        internal static bool IsData(this Opcode opcode) => opcode == Opcode.Text || opcode == Opcode.Binary;

        internal static bool IsPortNumber(this int value) => value > 0 && value < 65536;

        internal static bool IsReserved(this ushort code) => code == 1004 || code == 1005 || code == 1006 || code == 1015;

        internal static bool IsReserved(this CloseStatusCode code) => code == CloseStatusCode.Undefined || code == CloseStatusCode.NoStatus || code == CloseStatusCode.Abnormal ||
                                                                      code == CloseStatusCode.TlsHandshakeFailure;

        internal static bool IsSupported(this byte opcode) => Enum.IsDefined(typeof(Opcode), opcode);

        internal static bool IsText(this string value) {
            int length = value.Length;

            for (int i = 0; i < length; i++) {
                char c = value[i];

                if (c < ' ' && !"\r\n\t".Contains(c)) {
                    return false;
                }

                switch (c) {
                    case '\u007f':
                        return false;

                    case '\n':
                        if (++i < length) {
                            c = value[i];

                            if (!" \t".Contains(c)) {
                                return false;
                            }
                        }

                        break;
                }
            }

            return true;
        }

        internal static bool IsToken(this string value) {
            foreach (char c in value) {
                if (c < ' ' || c >= '\u007f' || "()<>@,;:\\\"/[]?={} \t".Contains(c)) {
                    return false;
                }
            }

            return true;
        }

        internal static string Quote(this string value) => string.Format("\"{0}\"", value.Replace("\"", "\\\""));

        internal static byte[] ReadBytes(this Stream stream, int length) {
            byte[] array = new byte[length];
            int num = 0;

            try {
                int num2 = 0;

                while (length > 0) {
                    num2 = stream.Read(array, num, length);

                    if (num2 == 0) {
                        break;
                    }

                    num += num2;
                    length -= num2;
                }
            } catch { }

            return array.SubArray(0, num);
        }

        internal static byte[] ReadBytes(this Stream stream, long length, int bufferLength) {
            using (MemoryStream memoryStream = new()) {
                try {
                    byte[] buffer = new byte[bufferLength];
                    int num = 0;

                    while (length > 0) {
                        if (length < bufferLength) {
                            bufferLength = (int)length;
                        }

                        num = stream.Read(buffer, 0, bufferLength);

                        if (num == 0) {
                            break;
                        }

                        memoryStream.Write(buffer, 0, num);
                        length -= num;
                    }
                } catch { }

                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }

        internal static void ReadBytesAsync(this Stream stream, int length, Action<byte[]> completed, Action<Exception> error) {
            byte[] buff = new byte[length];
            int offset = 0;
            AsyncCallback callback = null;

            callback = delegate(IAsyncResult ar) {
                try {
                    int num = stream.EndRead(ar);

                    if (num == 0 || num == length) {
                        if (completed != null) {
                            completed(buff.SubArray(0, offset + num));
                        }
                    } else {
                        offset += num;
                        length -= num;
                        stream.BeginRead(buff, offset, length, callback, null);
                    }
                } catch (Exception obj2) {
                    if (error != null) {
                        error(obj2);
                    }
                }
            };

            try {
                stream.BeginRead(buff, offset, length, callback, null);
            } catch (Exception obj) {
                if (error != null) {
                    error(obj);
                }
            }
        }

        internal static void ReadBytesAsync(this Stream stream, long length, int bufferLength, Action<byte[]> completed, Action<Exception> error) {
            MemoryStream dest = new();
            byte[] buff = new byte[bufferLength];
            Action<long> read = null;

            read = delegate(long len) {
                if (len < bufferLength) {
                    bufferLength = (int)len;
                }

                stream.BeginRead(buff, 0, bufferLength, delegate(IAsyncResult ar) {
                    try {
                        int num = stream.EndRead(ar);

                        if (num > 0) {
                            dest.Write(buff, 0, num);
                        }

                        if (num == 0 || num == len) {
                            if (completed != null) {
                                dest.Close();
                                completed(dest.ToArray());
                            }

                            dest.Dispose();
                        } else {
                            read(len - num);
                        }
                    } catch (Exception obj2) {
                        dest.Dispose();

                        if (error != null) {
                            error(obj2);
                        }
                    }
                }, null);
            };

            try {
                read(length);
            } catch (Exception obj) {
                dest.Dispose();

                if (error != null) {
                    error(obj);
                }
            }
        }

        internal static string RemovePrefix(this string value, params string[] prefixes) {
            int num = 0;

            foreach (string text in prefixes) {
                if (value.StartsWith(text)) {
                    num = text.Length;
                    break;
                }
            }

            return num <= 0 ? value : value.Substring(num);
        }

        internal static T[] Reverse<T>(this T[] array) {
            int num = array.Length;
            T[] array2 = new T[num];
            int num2 = num - 1;

            for (int i = 0; i <= num2; i++) {
                array2[i] = array[num2 - i];
            }

            return array2;
        }

        internal static IEnumerable<string> SplitHeaderValue(this string value, params char[] separators) {
            int len = value.Length;
            string seps = new(separators);
            StringBuilder buff = new(32);
            bool escaped = false;
            bool quoted = false;

            for (int i = 0; i < len; i++) {
                char c = value[i];

                switch (c) {
                    case '"':
                        if (escaped) {
                            escaped = !escaped;
                        } else {
                            quoted = !quoted;
                        }

                        break;

                    case '\\':
                        if (i < len - 1 && value[i + 1] == '"') {
                            escaped = true;
                        }

                        break;

                    default:
                        if (seps.Contains(c) && !quoted) {
                            yield return buff.ToString();

                            buff.Length = 0;
                            continue;
                        }

                        break;
                }

                buff.Append(c);
            }

            if (buff.Length > 0) {
                yield return buff.ToString();
            }
        }

        internal static byte[] ToByteArray(this Stream stream) {
            using (MemoryStream memoryStream = new()) {
                stream.Position = 0L;
                stream.CopyTo(memoryStream, 1024);
                memoryStream.Close();
                return memoryStream.ToArray();
            }
        }

        internal static CompressionMethod ToCompressionMethod(this string value) {
            IEnumerator enumerator = Enum.GetValues(typeof(CompressionMethod)).GetEnumerator();

            try {
                while (enumerator.MoveNext()) {
                    CompressionMethod compressionMethod = (CompressionMethod)enumerator.Current;

                    if (compressionMethod.ToExtensionString() == value) {
                        return compressionMethod;
                    }
                }
            } finally {
                IDisposable disposable;

                if ((disposable = enumerator as IDisposable) != null) {
                    disposable.Dispose();
                }
            }

            return CompressionMethod.None;
        }

        internal static string ToExtensionString(this CompressionMethod method, params string[] parameters) {
            if (method == CompressionMethod.None) {
                return string.Empty;
            }

            string text = string.Format("permessage-{0}", method.ToString().ToLower());

            if (parameters == null || parameters.Length == 0) {
                return text;
            }

            return string.Format("{0}; {1}", text, parameters.ToString("; "));
        }

        internal static IPAddress ToIPAddress(this string hostnameOrAddress) {
            try {
                return Dns.GetHostAddresses(hostnameOrAddress)[0];
            } catch {
                return null;
            }
        }

        internal static List<TSource> ToList<TSource>(this IEnumerable<TSource> source) => new(source);

        internal static ushort ToUInt16(this byte[] source, ByteOrder sourceOrder) => BitConverter.ToUInt16(source.ToHostOrder(sourceOrder), 0);

        internal static ulong ToUInt64(this byte[] source, ByteOrder sourceOrder) => BitConverter.ToUInt64(source.ToHostOrder(sourceOrder), 0);

        internal static string TrimEndSlash(this string value) {
            value = value.TrimEnd('/');
            return value.Length <= 0 ? "/" : value;
        }

        internal static bool TryCreateWebSocketUri(this string uriString, out Uri result, out string message) {
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

            if (!(scheme == "ws") && !(scheme == "wss")) {
                message = "The scheme part isn't 'ws' or 'wss': " + uriString;
                return false;
            }

            if (uri.Fragment.Length > 0) {
                message = "Includes the fragment component: " + uriString;
                return false;
            }

            int port = uri.Port;

            if (port == 0) {
                message = "The port part is zero: " + uriString;
                return false;
            }

            result = port == -1 ? new Uri(string.Format("{0}://{1}:{2}{3}", scheme, uri.Host, !(scheme == "ws") ? 443 : 80, uri.PathAndQuery)) : uri;
            message = string.Empty;
            return true;
        }

        internal static string Unquote(this string value) {
            int num = value.IndexOf('"');

            if (num < 0) {
                return value;
            }

            int num2 = value.LastIndexOf('"');
            int num3 = num2 - num - 1;
            return num3 < 0 ? value : num3 != 0 ? value.Substring(num + 1, num3).Replace("\\\"", "\"") : string.Empty;
        }

        internal static string UTF8Decode(this byte[] bytes) {
            try {
                return Encoding.UTF8.GetString(bytes);
            } catch {
                return null;
            }
        }

        internal static byte[] UTF8Encode(this string s) => Encoding.UTF8.GetBytes(s);

        internal static void WriteBytes(this Stream stream, byte[] bytes, int bufferLength) {
            using (MemoryStream source = new(bytes)) {
                source.CopyTo(stream, bufferLength);
            }
        }

        internal static void WriteBytesAsync(this Stream stream, byte[] bytes, int bufferLength, Action completed, Action<Exception> error) {
            MemoryStream input = new(bytes);

            input.CopyToAsync(stream, bufferLength, delegate {
                if (completed != null) {
                    completed();
                }

                input.Dispose();
            }, delegate(Exception ex) {
                input.Dispose();

                if (error != null) {
                    error(ex);
                }
            });
        }

        public static bool Contains(this string value, params char[] chars) => chars == null || chars.Length == 0 || value != null && value.Length != 0 && value.IndexOfAny(chars) > -1;

        public static bool Contains(this NameValueCollection collection, string name) => collection != null && collection.Count > 0 && collection[name] != null;

        public static bool Contains(this NameValueCollection collection, string name, string value) {
            if (collection == null || collection.Count == 0) {
                return false;
            }

            string text = collection[name];

            if (text == null) {
                return false;
            }

            string[] array = text.Split(',');

            foreach (string text2 in array) {
                if (text2.Trim().Equals(value, StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
            }

            return false;
        }

        public static void Emit(this EventHandler eventHandler, object sender, EventArgs e) {
            if (eventHandler != null) {
                eventHandler(sender, e);
            }
        }

        public static void Emit<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs e) where TEventArgs : EventArgs {
            if (eventHandler != null) {
                eventHandler(sender, e);
            }
        }

        public static CookieCollection GetCookies(this NameValueCollection headers, bool response) {
            string name = !response ? "Cookie" : "Set-Cookie";
            return headers == null || !headers.Contains(name) ? new CookieCollection() : CookieCollection.Parse(headers[name], response);
        }

        public static string GetDescription(this HttpStatusCode code) => ((int)code).GetStatusDescription();

        public static string GetStatusDescription(this int code) {
            switch (code) {
                case 100:
                    return "Continue";

                case 101:
                    return "Switching Protocols";

                case 102:
                    return "Processing";

                case 200:
                    return "OK";

                case 201:
                    return "Created";

                case 202:
                    return "Accepted";

                case 203:
                    return "Non-Authoritative Information";

                case 204:
                    return "No Content";

                case 205:
                    return "Reset Content";

                case 206:
                    return "Partial Content";

                case 207:
                    return "Multi-Status";

                case 300:
                    return "Multiple Choices";

                case 301:
                    return "Moved Permanently";

                case 302:
                    return "Found";

                case 303:
                    return "See Other";

                case 304:
                    return "Not Modified";

                case 305:
                    return "Use Proxy";

                case 307:
                    return "Temporary Redirect";

                case 400:
                    return "Bad Request";

                case 401:
                    return "Unauthorized";

                case 402:
                    return "Payment Required";

                case 403:
                    return "Forbidden";

                case 404:
                    return "Not Found";

                case 405:
                    return "Method Not Allowed";

                case 406:
                    return "Not Acceptable";

                case 407:
                    return "Proxy Authentication Required";

                case 408:
                    return "Request Timeout";

                case 409:
                    return "Conflict";

                case 410:
                    return "Gone";

                case 411:
                    return "Length Required";

                case 412:
                    return "Precondition Failed";

                case 413:
                    return "Request Entity Too Large";

                case 414:
                    return "Request-Uri Too Long";

                case 415:
                    return "Unsupported Media Type";

                case 416:
                    return "Requested Range Not Satisfiable";

                case 417:
                    return "Expectation Failed";

                case 422:
                    return "Unprocessable Entity";

                case 423:
                    return "Locked";

                case 424:
                    return "Failed Dependency";

                case 500:
                    return "Internal Server Error";

                case 501:
                    return "Not Implemented";

                case 502:
                    return "Bad Gateway";

                case 503:
                    return "Service Unavailable";

                case 504:
                    return "Gateway Timeout";

                case 505:
                    return "Http Version Not Supported";

                case 507:
                    return "Insufficient Storage";

                default:
                    return string.Empty;
            }
        }

        public static bool IsCloseStatusCode(this ushort value) => value > 999 && value < 5000;

        public static bool IsEnclosedIn(this string value, char c) => value != null && value.Length > 1 && value[0] == c && value[value.Length - 1] == c;

        public static bool IsHostOrder(this ByteOrder order) => !(BitConverter.IsLittleEndian ^ order == ByteOrder.Little);

        public static bool IsLocal(this IPAddress address) {
            if (address == null) {
                return false;
            }

            if (address.Equals(IPAddress.Any)) {
                return true;
            }

            if (address.Equals(IPAddress.Loopback)) {
                return true;
            }

            if (Socket.OSSupportsIPv6) {
                if (address.Equals(IPAddress.IPv6Any)) {
                    return true;
                }

                if (address.Equals(IPAddress.IPv6Loopback)) {
                    return true;
                }
            }

            string hostName = Dns.GetHostName();
            IPAddress[] hostAddresses = Dns.GetHostAddresses(hostName);
            IPAddress[] array = hostAddresses;

            foreach (IPAddress obj in array) {
                if (address.Equals(obj)) {
                    return true;
                }
            }

            return false;
        }

        public static bool IsNullOrEmpty(this string value) => value == null || value.Length == 0;

        public static bool IsPredefinedScheme(this string value) {
            if (value == null || value.Length < 2) {
                return false;
            }

            char c = value[0];

            if (c == 'h') {
                return value == "http" || value == "https";
            }

            if (c == 'w') {
                return value == "ws" || value == "wss";
            }

            if (c == 'f') {
                return value == "file" || value == "ftp";
            }

            if (c == 'n') {
                c = value[1];
                return c != 'e' ? value == "nntp" : value == "news" || value == "net.pipe" || value == "net.tcp";
            }

            return c == 'g' && value == "gopher" || c == 'm' && value == "mailto";
        }

        public static bool IsUpgradeTo(this HttpListenerRequest request, string protocol) {
            if (request == null) {
                throw new ArgumentNullException("request");
            }

            if (protocol == null) {
                throw new ArgumentNullException("protocol");
            }

            if (protocol.Length == 0) {
                throw new ArgumentException("An empty string.", "protocol");
            }

            return request.Headers.Contains("Upgrade", protocol) && request.Headers.Contains("Connection", "Upgrade");
        }

        public static bool MaybeUri(this string value) {
            if (value == null || value.Length == 0) {
                return false;
            }

            int num = value.IndexOf(':');

            if (num == -1) {
                return false;
            }

            if (num >= 10) {
                return false;
            }

            return value.Substring(0, num).IsPredefinedScheme();
        }

        public static T[] SubArray<T>(this T[] array, int startIndex, int length) {
            int num;

            if (array == null || (num = array.Length) == 0) {
                return new T[0];
            }

            if (startIndex < 0 || length <= 0 || startIndex + length > num) {
                return new T[0];
            }

            if (startIndex == 0 && length == num) {
                return array;
            }

            T[] array2 = new T[length];
            Array.Copy(array, startIndex, array2, 0, length);
            return array2;
        }

        public static T[] SubArray<T>(this T[] array, long startIndex, long length) {
            long longLength;

            if (array == null || (longLength = array.LongLength) == 0) {
                return new T[0];
            }

            if (startIndex < 0 || length <= 0 || startIndex + length > longLength) {
                return new T[0];
            }

            if (startIndex == 0 && length == longLength) {
                return array;
            }

            T[] array2 = new T[length];
            Array.Copy(array, startIndex, array2, 0L, length);
            return array2;
        }

        public static void Times(this int n, Action action) {
            if (n > 0 && action != null) {
                ((ulong)n).times(action);
            }
        }

        public static void Times(this long n, Action action) {
            if (n > 0 && action != null) {
                ((ulong)n).times(action);
            }
        }

        public static void Times(this uint n, Action action) {
            if (n != 0 && action != null) {
                times(n, action);
            }
        }

        public static void Times(this ulong n, Action action) {
            if (n != 0 && action != null) {
                n.times(action);
            }
        }

        public static void Times(this int n, Action<int> action) {
            if (n > 0 && action != null) {
                for (int i = 0; i < n; i++) {
                    action(i);
                }
            }
        }

        public static void Times(this long n, Action<long> action) {
            if (n > 0 && action != null) {
                for (long num = 0L; num < n; num++) {
                    action(num);
                }
            }
        }

        public static void Times(this uint n, Action<uint> action) {
            if (n != 0 && action != null) {
                for (uint num = 0u; num < n; num++) {
                    action(num);
                }
            }
        }

        public static void Times(this ulong n, Action<ulong> action) {
            if (n != 0 && action != null) {
                for (ulong num = 0uL; num < n; num++) {
                    action(num);
                }
            }
        }

        public static T To<T>(this byte[] source, ByteOrder sourceOrder) where T : struct {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            if (source.Length == 0) {
                return default;
            }

            Type typeFromHandle = typeof(T);
            byte[] value = source.ToHostOrder(sourceOrder);

            return typeFromHandle == typeof(bool) ? (T)(object)BitConverter.ToBoolean(value, 0) :
                   typeFromHandle == typeof(char) ? (T)(object)BitConverter.ToChar(value, 0) :
                   typeFromHandle == typeof(double) ? (T)(object)BitConverter.ToDouble(value, 0) :
                   typeFromHandle == typeof(short) ? (T)(object)BitConverter.ToInt16(value, 0) :
                   typeFromHandle == typeof(int) ? (T)(object)BitConverter.ToInt32(value, 0) :
                   typeFromHandle == typeof(long) ? (T)(object)BitConverter.ToInt64(value, 0) :
                   typeFromHandle == typeof(float) ? (T)(object)BitConverter.ToSingle(value, 0) :
                   typeFromHandle == typeof(ushort) ? (T)(object)BitConverter.ToUInt16(value, 0) :
                   typeFromHandle == typeof(uint) ? (T)(object)BitConverter.ToUInt32(value, 0) :
                   typeFromHandle != typeof(ulong) ? default : (T)(object)BitConverter.ToUInt64(value, 0);
        }

        public static byte[] ToByteArray<T>(this T value, ByteOrder order) where T : struct {
            Type typeFromHandle = typeof(T);

            byte[] array = typeFromHandle == typeof(bool) ? BitConverter.GetBytes((bool)(object)value) :
                           typeFromHandle == typeof(byte) ? new byte[1] { (byte)(object)value } :
                           typeFromHandle == typeof(char) ? BitConverter.GetBytes((char)(object)value) :
                           typeFromHandle == typeof(double) ? BitConverter.GetBytes((double)(object)value) :
                           typeFromHandle == typeof(short) ? BitConverter.GetBytes((short)(object)value) :
                           typeFromHandle == typeof(int) ? BitConverter.GetBytes((int)(object)value) :
                           typeFromHandle == typeof(long) ? BitConverter.GetBytes((long)(object)value) :
                           typeFromHandle == typeof(float) ? BitConverter.GetBytes((float)(object)value) :
                           typeFromHandle == typeof(ushort) ? BitConverter.GetBytes((ushort)(object)value) :
                           typeFromHandle == typeof(uint) ? BitConverter.GetBytes((uint)(object)value) :
                           typeFromHandle != typeof(ulong) ? WebSocket.EmptyBytes : BitConverter.GetBytes((ulong)(object)value);

            if (array.Length > 1 && !order.IsHostOrder()) {
                Array.Reverse(array);
            }

            return array;
        }

        public static byte[] ToHostOrder(this byte[] source, ByteOrder sourceOrder) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }

            return source.Length <= 1 || sourceOrder.IsHostOrder() ? source : source.Reverse();
        }

        public static string ToString<T>(this T[] array, string separator) {
            if (array == null) {
                throw new ArgumentNullException("array");
            }

            int num = array.Length;

            if (num == 0) {
                return string.Empty;
            }

            if (separator == null) {
                separator = string.Empty;
            }

            StringBuilder buff = new(64);

            (num - 1).Times(delegate(int i) {
                buff.AppendFormat("{0}{1}", array[i].ToString(), separator);
            });

            buff.Append(array[num - 1]);
            return buff.ToString();
        }

        public static Uri ToUri(this string uriString) {
            Uri result;
            Uri.TryCreate(uriString, uriString.MaybeUri() ? UriKind.Absolute : UriKind.Relative, out result);
            return result;
        }

        public static string UrlDecode(this string value) => value == null || value.Length <= 0 ? value : HttpUtility.UrlDecode(value);

        public static string UrlEncode(this string value) => value == null || value.Length <= 0 ? value : HttpUtility.UrlEncode(value);

        public static void WriteContent(this HttpListenerResponse response, byte[] content) {
            if (response == null) {
                throw new ArgumentNullException("response");
            }

            if (content == null) {
                throw new ArgumentNullException("content");
            }

            long longLength = content.LongLength;

            if (longLength == 0) {
                response.Close();
                return;
            }

            response.ContentLength64 = longLength;
            Stream outputStream = response.OutputStream;

            if (longLength <= int.MaxValue) {
                outputStream.Write(content, 0, (int)longLength);
            } else {
                outputStream.WriteBytes(content, 1024);
            }

            outputStream.Close();
        }
    }
}