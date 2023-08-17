using System;

namespace Platform.Library.ClientProtocol.API {
    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolDictionaryAttribute : Attribute {
        public ProtocolDictionaryAttribute(bool optionalKey, bool variedKey, bool optionalValue, bool variedValue) {
            OptionalKey = optionalKey;
            VariedKey = variedKey;
            OptionalValue = optionalValue;
            VariedValue = variedValue;
        }

        public bool OptionalKey { get; private set; }

        public bool VariedKey { get; private set; }

        public bool OptionalValue { get; private set; }

        public bool VariedValue { get; private set; }
    }
}