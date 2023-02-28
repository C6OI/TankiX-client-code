using System;
using System.Text;
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

        public void Init(Protocol protocol) {
            elementCodec = protocol.GetCodec(elementCodecInfo);
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            Array array = (Array)data;
            int length = array.Length;
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, length);

            for (int i = 0; i < length; i++) {
                elementCodec.Encode(protocolBuffer, array.GetValue(i));
            }
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            int i = 0;
            Array array = null;
            int num = 0;

            try {
                num = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
                array = Array.CreateInstance(elementType, num);

                for (; i < num; i++) {
                    object value = elementCodec.Decode(protocolBuffer);
                    array.SetValue(value, i);
                }

                return array;
            } catch (Exception innerException) {
                StringBuilder stringBuilder = new();

                for (int j = 0; j <= i; j++) {
                    object value2 = array.GetValue(j);
                    stringBuilder.Append(j);
                    stringBuilder.Append(") ");
                    stringBuilder.Append(value2);
                    stringBuilder.Append("\n");
                }

                throw new Exception("Array decode failed; ElementType: " + elementType.Name + " length: " + num + " decodedElements: " + stringBuilder, innerException);
            }
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance) {
            throw new NotImplementedException();
        }
    }
}