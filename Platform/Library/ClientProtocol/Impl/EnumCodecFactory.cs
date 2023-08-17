using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class EnumCodecFactory : CodecFactory {
        readonly EnumCodec enumCodec;

        public EnumCodecFactory() => enumCodec = new EnumCodec();

        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs) {
            Type type = codecInfoWithAttrs.Info.type;

            if (type.IsEnum) {
                return enumCodec;
            }

            return null;
        }
    }
}