using System;
using System.Collections.Specialized;
using System.Text;

namespace WebSocketSharp.Net {
    internal abstract class AuthenticationBase {
        internal NameValueCollection Parameters;

        protected AuthenticationBase(AuthenticationSchemes scheme, NameValueCollection parameters) {
            Scheme = scheme;
            Parameters = parameters;
        }

        public string Algorithm => Parameters["algorithm"];

        public string Nonce => Parameters["nonce"];

        public string Opaque => Parameters["opaque"];

        public string Qop => Parameters["qop"];

        public string Realm => Parameters["realm"];

        public AuthenticationSchemes Scheme { get; }

        internal static string CreateNonceValue() {
            byte[] array = new byte[16];
            Random random = new();
            random.NextBytes(array);
            StringBuilder stringBuilder = new(32);
            byte[] array2 = array;

            foreach (byte b in array2) {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        internal static NameValueCollection ParseParameters(string value) {
            NameValueCollection nameValueCollection = new();

            foreach (string item in value.SplitHeaderValue(',')) {
                int num = item.IndexOf('=');
                string name = num <= 0 ? null : item.Substring(0, num).Trim();
                string val = num >= 0 ? num >= item.Length - 1 ? string.Empty : item.Substring(num + 1).Trim().Trim('"') : item.Trim().Trim('"');
                nameValueCollection.Add(name, val);
            }

            return nameValueCollection;
        }

        internal abstract string ToBasicString();

        internal abstract string ToDigestString();

        public override string ToString() => Scheme == AuthenticationSchemes.Basic ? ToBasicString() : Scheme != AuthenticationSchemes.Digest ? string.Empty : ToDigestString();
    }
}