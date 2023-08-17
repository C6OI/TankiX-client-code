using System;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class ArrayCodec : Codec {
        readonly CodecInfoWithAttributes elementCodecInfo;
        readonly Type elementType;

        Codec elementCodec;

        public ArrayCodec(Type elementType, CodecInfoWithAttributes elementCodecInfo) {
            this.elementType = elementType;
            this.elementCodecInfo = elementCodecInfo;
        }

        public void Init(Protocol protocol) => elementCodec = protocol.GetCodec(elementCodecInfo);

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            Array array = (Array)data;
            int length = array.Length;
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, length);

            for (int i = 0; i < length; i++) {
                elementCodec.Encode(protocolBuffer, array.GetValue(i));
            }
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            int num = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
            Array array = Array.CreateInstance(elementType, num);

            for (int i = 0; i < num; i++) {
                object value = elementCodec.Decode(protocolBuffer);
                array.SetValue(value, i);
            }

            return array;
        }
    }
}