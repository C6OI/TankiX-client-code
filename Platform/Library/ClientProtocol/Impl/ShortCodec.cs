using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class ShortCodec : NotOptionalCodec {
        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            protocolBuffer.Writer.Write((short)data);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) => protocolBuffer.Reader.ReadInt16();
    }
}