using System;

namespace Platform.Library.ClientProtocol.API {
    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolVariedAttribute : Attribute {
        public readonly bool value;

        public ProtocolVariedAttribute() => value = true;

        public ProtocolVariedAttribute(bool value) => this.value = value;
    }
}