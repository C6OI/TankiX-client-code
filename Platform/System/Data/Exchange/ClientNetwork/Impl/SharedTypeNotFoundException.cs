using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class SharedTypeNotFoundException : Exception {
        public SharedTypeNotFoundException(long id, Type type)
            : base(string.Format("Shared type with UID {0} was not found in registry {1}.", id, type)) { }
    }
}