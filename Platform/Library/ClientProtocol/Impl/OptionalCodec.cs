using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class OptionalCodec : Codec {
        readonly Codec codec;

        public OptionalCodec(Codec codec) => this.codec = codec;

        public void Init(Protocol protocol) => codec.Init(protocol);

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            if (data == null) {
                protocolBuffer.OptionalMap.Add(true);
                return;
            }

            protocolBuffer.OptionalMap.Add(false);
            codec.Encode(protocolBuffer, data);
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            if (protocolBuffer.OptionalMap.Get()) {
                return null;
            }

            return codec.Decode(protocolBuffer);
        }
    }
}