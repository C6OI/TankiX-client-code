using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class FloatCodec : NotOptionalCodec {
        public static int ENCODE_NAN_ERRORS;

        public static int DECODE_NAN_ERRORS;

        public static Exception encodeErrorStack;

        public override void Init(Protocol protocol) { }

        public override void Encode(ProtocolBuffer protocolBuffer, object data) {
            base.Encode(protocolBuffer, data);
            float num = (float)data;

            if (float.IsNaN(num) || float.IsInfinity(num)) {
                ENCODE_NAN_ERRORS++;
                encodeErrorStack = new Exception();
            }

            protocolBuffer.Writer.Write(num);
        }

        public override object Decode(ProtocolBuffer protocolBuffer) {
            float num = protocolBuffer.Reader.ReadSingle();

            if (float.IsNaN(num) || float.IsInfinity(num)) {
                DECODE_NAN_ERRORS++;
            }

            return num;
        }
    }
}