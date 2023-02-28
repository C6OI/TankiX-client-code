using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class VariedStructCodec : NotOptionalCodec {
        Protocol protocol;

        public override void Init(Protocol protocol) {
            this.protocol = protocol;
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            Type type = data.GetType();
            long uidByType = protocol.GetUidByType(type);
            protocolBuffer.Writer.Write(uidByType);
            ProtocolBuffer protocolBuffer2 = protocol.NewProtocolBuffer();
            Codec codec = protocol.GetCodec(type);
            codec.Encode(protocolBuffer2, data);
            protocol.WrapPacket(protocolBuffer2, protocolBuffer.Data);
            protocol.FreeProtocolBuffer(protocolBuffer2);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            long uid = protocolBuffer.Reader.ReadInt64();
            Codec codec = protocol.GetCodec(uid);
            return codec.Decode(protocolBuffer);
        }
    }
}