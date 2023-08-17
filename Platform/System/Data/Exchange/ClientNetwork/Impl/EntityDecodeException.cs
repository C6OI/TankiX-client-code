using System;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class EntityDecodeException : Exception {
        public EntityDecodeException(long entityId, Exception e)
            : base("EntityId=" + entityId, e) { }
    }
}