using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using WebSocketSharp.Net;

namespace WebSocketSharp {
    internal class HttpResponse : HttpBase {
        HttpResponse(string code, string reason, Version version, NameValueCollection headers)
            : base(version, headers) {
            StatusCode = code;
            Reason = reason;
        }

        internal HttpResponse(HttpStatusCode code)
            : this(code, code.GetDescription()) { }

        internal HttpResponse(HttpStatusCode code, string reason)
            : this(((int)code).ToString(), reason, HttpVersion.Version11, new NameValueCollection()) => Headers["Server"] = "websocket-sharp/1.0";

        public CookieCollection Cookies => Headers.GetCookies(true);

        public bool HasConnectionClose => Headers.Contains("Connection", "close");

        public bool IsProxyAuthenticationRequired => StatusCode == "407";

        public bool IsRedirect => StatusCode == "301" || StatusCode == "302";

        public bool IsUnauthorized => StatusCode == "401";

        public bool IsWebSocketResponse {
            get {
                NameValueCollection headers = Headers;
                return ProtocolVersion > HttpVersion.Version10 && StatusCode == "101" && headers.Contains("Upgrade", "websocket") && headers.Contains("Connection", "Upgrade");
            }
        }

        public string Reason { get; }

        public string StatusCode { get; }

        internal static HttpResponse CreateCloseResponse(HttpStatusCode code) {
            HttpResponse httpResponse = new(code);
            httpResponse.Headers["Connection"] = "close";
            return httpResponse;
        }

        internal static HttpResponse CreateUnauthorizedResponse(string challenge) {
            HttpResponse httpResponse = new(HttpStatusCode.Unauthorized);
            httpResponse.Headers["WWW-Authenticate"] = challenge;
            return httpResponse;
        }

        internal static HttpResponse CreateWebSocketResponse() {
            HttpResponse httpResponse = new(HttpStatusCode.SwitchingProtocols);
            NameValueCollection headers = httpResponse.Headers;
            headers["Upgrade"] = "websocket";
            headers["Connection"] = "Upgrade";
            return httpResponse;
        }

        internal static HttpResponse Parse(string[] headerParts) {
            string[] array = headerParts[0].Split(new char[1] { ' ' }, 3);

            if (array.Length != 3) {
                throw new ArgumentException("Invalid status line: " + headerParts[0]);
            }

            WebHeaderCollection webHeaderCollection = new();

            for (int i = 1; i < headerParts.Length; i++) {
                webHeaderCollection.InternalSet(headerParts[i], true);
            }

            return new HttpResponse(array[1], array[2], new Version(array[0].Substring(5)), webHeaderCollection);
        }

        internal static HttpResponse Read(Stream stream, int millisecondsTimeout) => Read(stream, Parse, millisecondsTimeout);

        public void SetCookies(CookieCollection cookies) {
            if (cookies == null || cookies.Count == 0) {
                return;
            }

            NameValueCollection headers = Headers;

            foreach (Cookie item in cookies.Sorted) {
                headers.Add("Set-Cookie", item.ToResponseString());
            }
        }

        public override string ToString() {
            StringBuilder stringBuilder = new(64);
            stringBuilder.AppendFormat("HTTP/{0} {1} {2}{3}", ProtocolVersion, StatusCode, Reason, "\r\n");
            NameValueCollection headers = Headers;
            string[] allKeys = headers.AllKeys;

            foreach (string text in allKeys) {
                stringBuilder.AppendFormat("{0}: {1}{2}", text, headers[text], "\r\n");
            }

            stringBuilder.Append("\r\n");
            string entityBody = EntityBody;

            if (entityBody.Length > 0) {
                stringBuilder.Append(entityBody);
            }

            return stringBuilder.ToString();
        }
    }
}