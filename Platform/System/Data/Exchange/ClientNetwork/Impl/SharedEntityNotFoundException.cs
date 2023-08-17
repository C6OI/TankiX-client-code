using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class SharedEntityNotFoundException : Exception {
        public SharedEntityNotFoundException(long id)
            : base("Entity with ID " + id + " not found in engine.") { }
    }
}