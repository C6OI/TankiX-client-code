using System.Security.Principal;

namespace WebSocketSharp.Net {
    public class HttpBasicIdentity : GenericIdentity {
        internal HttpBasicIdentity(string username, string password)
            : base(username, "Basic") => Password = password;

        public virtual string Password { get; }
    }
}