using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class UnsharedEntityNotFoundException : Exception {
        public UnsharedEntityNotFoundException(long id)
            : base("id=" + id) { }
    }
}