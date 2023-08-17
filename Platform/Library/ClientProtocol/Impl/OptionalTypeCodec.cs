using System;
using System.Reflection;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class OptionalTypeCodec : NotOptionalCodec {
        readonly CodecInfoWithAttributes elementCodecInfo;

        readonly MethodInfo emptyMethod;

        readonly MethodInfo getMethod;

        readonly MethodInfo isPresentMethod;

        readonly MethodInfo ofMethod;
        readonly Type type;

        Codec elementCodec;

        public OptionalTypeCodec(Type type, CodecInfoWithAttributes elementCodecInfo) {
            this.type = type;
            this.elementCodecInfo = elementCodecInfo;
            emptyMethod = type.GetMethod("empty");
            ofMethod = type.GetMethod("of");
            isPresentMethod = type.GetMethod("IsPresent");
            getMethod = type.GetMethod("Get");
        }

        public override void Init(Protocol protocol) => elementCodec = protocol.GetCodec(elementCodecInfo);

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            if (data != null && (bool)isPresentMethod.Invoke(data, null)) {
                protocolBuffer.OptionalMap.Add(false);
                elementCodec.Encode(protocolBuffer, getMethod.Invoke(data, null));
            } else {
                protocolBuffer.OptionalMap.Add(true);
            }
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            if (protocolBuffer.OptionalMap.Get()) {
                return emptyMethod.Invoke(null, null);
            }

            object obj = elementCodec.Decode(protocolBuffer);
            return ofMethod.Invoke(null, new object[1] { obj });
        }
    }
}