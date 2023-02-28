using System.Collections.Specialized;
using System.Text;

namespace WebSocketSharp.Net {
    internal class AuthenticationChallenge : AuthenticationBase {
        AuthenticationChallenge(AuthenticationSchemes scheme, NameValueCollection parameters)
            : base(scheme, parameters) { }

        internal AuthenticationChallenge(AuthenticationSchemes scheme, string realm)
            : base(scheme, new NameValueCollection()) {
            Parameters["realm"] = realm;

            if (scheme == AuthenticationSchemes.Digest) {
                Parameters["nonce"] = CreateNonceValue();
                Parameters["algorithm"] = "MD5";
                Parameters["qop"] = "auth";
            }
        }

        public string Domain => Parameters["domain"];

        public string Stale => Parameters["stale"];

        internal static AuthenticationChallenge CreateBasicChallenge(string realm) => new(AuthenticationSchemes.Basic, realm);

        internal static AuthenticationChallenge CreateDigestChallenge(string realm) => new(AuthenticationSchemes.Digest, realm);

        internal static AuthenticationChallenge Parse(string value) {
            string[] array = value.Split(new char[1] { ' ' }, 2);

            if (array.Length != 2) {
                return null;
            }

            string text = array[0].ToLower();

            return text == "basic" ? new AuthenticationChallenge(AuthenticationSchemes.Basic, ParseParameters(array[1])) :
                   !(text == "digest") ? null : new AuthenticationChallenge(AuthenticationSchemes.Digest, ParseParameters(array[1]));
        }

        internal override string ToBasicString() => string.Format("Basic realm=\"{0}\"", Parameters["realm"]);

        internal override string ToDigestString() {
            StringBuilder stringBuilder = new(128);
            string text = Parameters["domain"];

            if (text != null) {
                stringBuilder.AppendFormat("Digest realm=\"{0}\", domain=\"{1}\", nonce=\"{2}\"", Parameters["realm"], text, Parameters["nonce"]);
            } else {
                stringBuilder.AppendFormat("Digest realm=\"{0}\", nonce=\"{1}\"", Parameters["realm"], Parameters["nonce"]);
            }

            string text2 = Parameters["opaque"];

            if (text2 != null) {
                stringBuilder.AppendFormat(", opaque=\"{0}\"", text2);
            }

            string text3 = Parameters["stale"];

            if (text3 != null) {
                stringBuilder.AppendFormat(", stale={0}", text3);
            }

            string text4 = Parameters["algorithm"];

            if (text4 != null) {
                stringBuilder.AppendFormat(", algorithm={0}", text4);
            }

            string text5 = Parameters["qop"];

            if (text5 != null) {
                stringBuilder.AppendFormat(", qop=\"{0}\"", text5);
            }

            return stringBuilder.ToString();
        }
    }
}