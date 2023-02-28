using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class VariedCodecFactory : CodecFactory {
        readonly VariedStructCodec structCodec;

        readonly VariedTypeCodec typeCodec;

        public VariedCodecFactory() {
            structCodec = new VariedStructCodec();
            typeCodec = new VariedTypeCodec();
        }

        public Codec CreateCodec(Protocol protocol, CodecInfoWithAttributes codecInfoWithAttrs) {
            if (codecInfoWithAttrs.Info.varied) {
                object result;

                if (codecInfoWithAttrs.Info.type == typeof(Type)) {
                    Codec codec = typeCodec;
                    result = codec;
                } else {
                    result = structCodec;
                }

                return (Codec)result;
            }

            return null;
        }
    }
}