using System;

namespace WebSocketSharp.Net {
    public class NetworkCredential {
        string _domain;

        string _password;

        string[] _roles;

        public NetworkCredential(string userName, string password)
            : this(userName, password, null, null) { }

        public NetworkCredential(string userName, string password, string domain, params string[] roles) {
            if (userName == null) {
                throw new ArgumentNullException("userName");
            }

            if (userName.Length == 0) {
                throw new ArgumentException("An empty string.", "userName");
            }

            UserName = userName;
            _password = password;
            _domain = domain;
            _roles = roles;
        }

        public string Domain {
            get => _domain ?? string.Empty;
            internal set => _domain = value;
        }

        public string Password {
            get => _password ?? string.Empty;
            internal set => _password = value;
        }

        public string[] Roles {
            get => _roles ?? (_roles = new string[0]);
            internal set => _roles = value;
        }

        public string UserName { get; internal set; }
    }
}