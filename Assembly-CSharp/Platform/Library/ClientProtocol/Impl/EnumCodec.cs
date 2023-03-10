using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class EnumCodec : NotOptionalCodec {
        readonly Type enumType;

        public EnumCodec(Type enumType) => this.enumType = enumType;

        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            Enum @enum = (Enum)data;

            if (@enum.GetTypeCode() != TypeCode.Byte) {
                throw new UnsupportedEnumTypeCodeException(@enum.GetTypeCode());
            }

            protocolBuffer.Writer.Write((byte)data);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) => Enum.ToObject(enumType, protocolBuffer.Reader.ReadByte());
    }
}