using System;
using System.Collections;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class ListCodec : Codec {
        readonly CodecInfoWithAttributes elementCodecInfo;
        readonly Type type;

        Codec elementCodec;

        public ListCodec(Type type, CodecInfoWithAttributes elementCodecInfo) {
            this.type = type;
            this.elementCodecInfo = elementCodecInfo;
        }

        public void Init(Protocol protocol) => elementCodec = protocol.GetCodec(elementCodecInfo);

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            IList list = (IList)data;
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, list.Count);

            if (list.Count <= 0) {
                return;
            }

            foreach (object item in list) {
                elementCodec.Encode(protocolBuffer, item);
            }
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            IList list = (IList)Activator.CreateInstance(type);
            int num = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);

            if (num > 0) {
                for (int i = 0; i < num; i++) {
                    object value = elementCodec.Decode(protocolBuffer);
                    list.Add(value);
                }
            }

            return list;
        }
    }
}