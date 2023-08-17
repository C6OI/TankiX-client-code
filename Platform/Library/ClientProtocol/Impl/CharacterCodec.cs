using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class CharacterCodec : NotOptionalCodec {
        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            protocolBuffer.Writer.Write((char)data);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) => protocolBuffer.Reader.ReadChar();
    }
}