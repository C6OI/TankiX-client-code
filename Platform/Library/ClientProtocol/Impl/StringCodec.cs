using System.Text;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class StringCodec : NotOptionalCodec {
        static readonly Encoding Encoding = Encoding.UTF8;

        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            string s = (string)data;
            int byteCount = Encoding.GetByteCount(s);
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, byteCount);
            protocolBuffer.Writer.Write(Encoding.GetBytes(s), 0, byteCount);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            int count = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
            byte[] bytes = protocolBuffer.Reader.ReadBytes(count);
            return Encoding.GetString(bytes);
        }
    }
}