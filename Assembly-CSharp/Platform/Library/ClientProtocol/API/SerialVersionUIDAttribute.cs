using System;

namespace Platform.Library.ClientProtocol.API {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class SerialVersionUIDAttribute : Attribute {
        public readonly long value;

        public SerialVersionUIDAttribute(long value) => this.value = value;
    }
}