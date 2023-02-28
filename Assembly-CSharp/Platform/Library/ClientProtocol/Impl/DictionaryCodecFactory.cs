using System;
using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class DictionaryCodecFactory : CodecFactory {
        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs) {
            Type type = codecInfoWithAttrs.Info.type;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>)) {
                Type type2 = type.GetGenericArguments()[0];
                Type type3 = type.GetGenericArguments()[1];
                bool optional = false;
                bool varied = false;
                bool optional2 = false;
                bool varied2 = false;

                if (codecInfoWithAttrs.IsAttributePresent<ProtocolDictionaryAttribute>()) {
                    ProtocolDictionaryAttribute attribute = codecInfoWithAttrs.GetAttribute<ProtocolDictionaryAttribute>();
                    optional = attribute.OptionalKey;
                    varied = attribute.VariedKey;
                    optional2 = attribute.OptionalValue;
                    varied2 = attribute.VariedValue;
                }

                CodecInfoWithAttributes keyRequest = new(type2, optional, varied);
                CodecInfoWithAttributes valueRequest = new(type3, optional2, varied2);
                return new DictionaryCodec(type, keyRequest, valueRequest);
            }

            return null;
        }
    }
}