using System;
using System.Net;

namespace WebSocketSharp.Net {
    internal sealed class HttpListenerPrefix {
        readonly string _original;

        ushort _port;

        internal HttpListenerPrefix(string uriPrefix) {
            _original = uriPrefix;
            parse(uriPrefix);
        }

        public IPAddress[] Addresses { get; set; }

        public string Host { get; private set; }

        public bool IsSecure { get; private set; }

        public HttpListener Listener { get; set; }

        public string Path { get; private set; }

        public int Port => _port;

        void parse(string uriPrefix) {
            int num = !uriPrefix.StartsWith("https://") ? 80 : 443;

            if (num == 443) {
                IsSecure = true;
            }

            int length = uriPrefix.Length;
            int num2 = uriPrefix.IndexOf(':') + 3;
            int num3 = uriPrefix.IndexOf(':', num2, length - num2);
            int num4 = 0;

            if (num3 > 0) {
                num4 = uriPrefix.IndexOf('/', num3, length - num3);
                Host = uriPrefix.Substring(num2, num3 - num2);
                _port = (ushort)int.Parse(uriPrefix.Substring(num3 + 1, num4 - num3 - 1));
            } else {
                num4 = uriPrefix.IndexOf('/', num2, length - num2);
                Host = uriPrefix.Substring(num2, num4 - num2);
                _port = (ushort)num;
            }

            Path = uriPrefix.Substring(num4);
            int length2 = Path.Length;

            if (length2 > 1) {
                Path = Path.Substring(0, length2 - 1);
            }
        }

        public static void CheckPrefix(string uriPrefix) {
            if (uriPrefix == null) {
                throw new ArgumentNullException("uriPrefix");
            }

            int length = uriPrefix.Length;

            if (length == 0) {
                throw new ArgumentException("An empty string.");
            }

            if (!uriPrefix.StartsWith("http://") && !uriPrefix.StartsWith("https://")) {
                throw new ArgumentException("The scheme isn't 'http' or 'https'.");
            }

            int num = uriPrefix.IndexOf(':') + 3;

            if (num >= length) {
                throw new ArgumentException("No host is specified.");
            }

            int num2 = uriPrefix.IndexOf(':', num, length - num);

            if (num == num2) {
                throw new ArgumentException("No host is specified.");
            }

            if (num2 > 0) {
                int num3 = uriPrefix.IndexOf('/', num2, length - num2);

                if (num3 == -1) {
                    throw new ArgumentException("No path is specified.");
                }

                int result;

                if (!int.TryParse(uriPrefix.Substring(num2 + 1, num3 - num2 - 1), out result) || !result.IsPortNumber()) {
                    throw new ArgumentException("An invalid port is specified.");
                }
            } else {
                int num4 = uriPrefix.IndexOf('/', num, length - num);

                if (num4 == -1) {
                    throw new ArgumentException("No path is specified.");
                }
            }

            if (uriPrefix[length - 1] != '/') {
                throw new ArgumentException("Ends without '/'.");
            }
        }

        public override bool Equals(object obj) {
            HttpListenerPrefix httpListenerPrefix = obj as HttpListenerPrefix;
            return httpListenerPrefix != null && httpListenerPrefix._original == _original;
        }

        public override int GetHashCode() => _original.GetHashCode();

        public override string ToString() => _original;
    }
}