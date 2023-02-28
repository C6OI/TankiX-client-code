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

        public bool OptionalKey { get; }

        public bool VariedKey { get; }

        public bool OptionalValue { get; }

        public bool VariedValue { get; }
    }
}