using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class VariedTypeCodec : NotOptionalCodec {
        Protocol protocol;

        public override void Init(Protocol protocol) {
            this.protocol = protocol;
        }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            Type cl = (Type)data;
            long uidByType = protocol.GetUidByType(cl);
            protocolBuffer.Writer.Write(uidByType);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            long uid = protocolBuffer.Reader.ReadInt64();
            return protocol.GetTypeByUid(uid);
        }
    }
}