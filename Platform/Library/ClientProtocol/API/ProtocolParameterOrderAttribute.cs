using System;

namespace Platform.Library.ClientProtocol.API {
    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolParameterOrderAttribute : Attribute {
        public ProtocolParameterOrderAttribute(int order) => Order = order;

        public int Order { get; set; }
    }
}