using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class FloatCodec : NotOptionalCodec {
        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            protocolBuffer.Writer.Write((float)data);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) => protocolBuffer.Reader.ReadSingle();
    }
}