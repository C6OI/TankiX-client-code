using System;
using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class EnumCodecFactory : CodecFactory {
        readonly Dictionary<Type, EnumCodec> codecs = new();

        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs) {
            Type type = codecInfoWithAttrs.Info.type;

            if (type.IsEnum) {
                EnumCodec value;

                if (codecs.TryGetValue(type, out value)) {
                    return value;
                }

                value = new EnumCodec(type);
                codecs[type] = value;
                return value;
            }

            return null;
        }
    }
}