using System;
using System.Collections.Generic;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class StructCodec : Codec {
        readonly List<PropertyRequest> requests;

        readonly Type type;
        int codecCount;

        List<PropertyCodec> codecs;

        public StructCodec(Type type, List<PropertyRequest> requests) {
            this.type = type;
            this.requests = requests;
        }

        public void Init(Protocol protocol) {
            codecCount = requests.Count;
            codecs = new List<PropertyCodec>(codecCount);

            for (int i = 0; i < codecCount; i++) {
                PropertyRequest propertyRequest = requests[i];
                Codec codec = protocol.GetCodec(propertyRequest.CodecInfoWithAttributes);
                codecs.Add(new PropertyCodec(codec, propertyRequest.PropertyInfo));
            }
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            for (int i = 0; i < codecCount; i++) {
                PropertyCodec propertyCodec = codecs[i];
                object value = propertyCodec.PropertyInfo.GetValue(data, null);
                propertyCodec.Codec.Encode(protocolBuffer, value);
            }
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            object obj = Activator.CreateInstance(type);

            for (int i = 0; i < codecCount; i++) {
                PropertyCodec propertyCodec = codecs[i];
                object value = propertyCodec.Codec.Decode(protocolBuffer);
                propertyCodec.PropertyInfo.SetValue(obj, value, null);
            }

            return obj;
        }

        public override string ToString() => string.Concat("StructCodec[", type, "]");
    }
}