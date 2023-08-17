using System;
using System.Reflection;

namespace Platform.Library.ClientProtocol.Impl {
    public class StructEncodeException : Exception {
        public StructEncodeException(Type structType, PropertyInfo prop, Exception e)
            : base(string.Format("structType={0} prop={1}", structType, prop), e) { }
    }
}