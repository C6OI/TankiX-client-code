using System;

namespace Platform.Library.ClientProtocol.API {
    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolCollectionAttribute : Attribute {
        public ProtocolCollectionAttribute(bool optional, bool varied) {
            Optional = optional;
            Varied = varied;
        }

        public bool Optional { get; private set; }

        public bool Varied { get; private set; }
    }
}