using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class SharedEntityAlreadyExistsException : Exception {
        public SharedEntityAlreadyExistsException(long id)
            : base("Entity with ID " + id + " already exists in engine.") { }
    }
}