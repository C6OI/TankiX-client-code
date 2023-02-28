using System.Collections.Specialized;
using System.Security.Principal;

namespace WebSocketSharp.Net {
    public class HttpDigestIdentity : GenericIdentity {
        readonly NameValueCollection _parameters;

        internal HttpDigestIdentity(NameValueCollection parameters)
            : base(parameters["username"], "Digest") => _parameters = parameters;

        public string Algorithm => _parameters["algorithm"];

        public string Cnonce => _parameters["cnonce"];

        public string Nc => _parameters["nc"];

        public string Nonce => _parameters["nonce"];

        public string Opaque => _parameters["opaque"];

        public string Qop => _parameters["qop"];

        public string Realm => _parameters["realm"];

        public string Response => _parameters["response"];

        public string Uri => _parameters["uri"];

        internal bool IsValid(string password, string realm, string method, string entity) {
            NameValueCollection nameValueCollection = new(_parameters);
            nameValueCollection["password"] = password;
            nameValueCollection["realm"] = realm;
            nameValueCollection["method"] = method;
            nameValueCollection["entity"] = entity;
            return _parameters["response"] == AuthenticationResponse.CreateRequestDigest(nameValueCollection);
        }
    }
}