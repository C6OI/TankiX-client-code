using System;

namespace Platform.Library.ClientProtocol.API {
    public class SerialVersionUidNotFoundException : Exception {
        public SerialVersionUidNotFoundException(Type type)
            : base("Type = " + type) { }
    }
}