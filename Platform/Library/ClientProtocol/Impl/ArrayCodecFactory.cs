using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class ArrayCodecFactory : CodecFactory {
        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs) {
            Type type = codecInfoWithAttrs.Info.type;

            if (type.IsArray) {
                Type elementType = type.GetElementType();
                bool optional = false;
                bool varied = false;

                if (codecInfoWithAttrs.IsAttributePresent<ProtocolCollectionAttribute>()) {
                    ProtocolCollectionAttribute attribute = codecInfoWithAttrs.GetAttribute<ProtocolCollectionAttribute>();
                    optional = attribute.Optional;
                    varied = attribute.Varied;
                }

                CodecInfoWithAttributes elementCodecInfo = new(type.GetElementType(), optional, varied);
                return new ArrayCodec(elementType, elementCodecInfo);
            }

            return null;
        }
    }
}