using System;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class OptionalTypeCodecFactory : CodecFactory {
        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs) {
            Type type = codecInfoWithAttrs.Info.type;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>)) {
                Type type2 = type.GetGenericArguments()[0];
                return new OptionalTypeCodec(type, new CodecInfoWithAttributes(type2, false, codecInfoWithAttrs.Info.varied));
            }

            return null;
        }
    }
}