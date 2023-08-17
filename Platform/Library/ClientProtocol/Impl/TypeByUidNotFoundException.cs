using System;

namespace Platform.Library.ClientProtocol.Impl {
    public class TypeByUidNotFoundException : Exception {
        public TypeByUidNotFoundException(long uid)
            : base("uid = " + uid) { }
    }
}