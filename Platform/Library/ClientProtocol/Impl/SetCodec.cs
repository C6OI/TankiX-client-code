using System;
using System.Collections;
using System.Reflection;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class SetCodec : Codec {
        readonly MethodInfo addMethod;

        readonly PropertyInfo countProperty;
        readonly CodecInfoWithAttributes elementCodecInfo;
        readonly Type type;

        Codec elementCodec;

        public SetCodec(Type type, CodecInfoWithAttributes elementCodecInfo) {
            this.type = type;
            this.elementCodecInfo = elementCodecInfo;
            addMethod = type.GetMethod("Add");
            countProperty = type.GetProperty("Count");
        }

        public void Init(Protocol protocol) => elementCodec = protocol.GetCodec(elementCodecInfo);

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            int num = (int)countProperty.GetValue(data, null);
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, num);

            if (num <= 0) {
                return;
            }

            foreach (object item in (IEnumerable)data) {
                elementCodec.Encode(protocolBuffer, item);
            }
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            object obj = Activator.CreateInstance(type);
            int num = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);

            if (num > 0) {
                for (int i = 0; i < num; i++) {
                    object obj2 = elementCodec.Decode(protocolBuffer);
                    addMethod.Invoke(obj, new object[1] { obj2 });
                }
            }

            return obj;
        }
    }
}