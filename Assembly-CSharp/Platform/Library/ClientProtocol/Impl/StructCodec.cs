using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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

                try {
                    object value = propertyCodec.PropertyInfo.GetValue(data, BindingFlags.Default, null, null, null);
                    propertyCodec.Codec.Encode(protocolBuffer, value);
                } catch (Exception innerException) {
                    throw new Exception("Property encoding exception, property=" + propertyCodec.PropertyInfo.Name + " type=" + propertyCodec.PropertyInfo.DeclaringType, innerException);
                }
            }
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            object obj = Activator.CreateInstance(type);
            DecodeToInstance(protocolBuffer, obj);
            return obj;
        }

        public void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance) {
            int i = 0;

            try {
                for (; i < codecCount; i++) {
                    PropertyCodec propertyCodec = codecs[i];
                    object value = propertyCodec.Codec.Decode(protocolBuffer);
                    propertyCodec.PropertyInfo.SetValue(instance, value, null);
                }
            } catch (Exception innerException) {
                StringBuilder stringBuilder = new();

                for (int j = 0; j <= i; j++) {
                    PropertyCodec propertyCodec2 = codecs[j];
                    stringBuilder.Append(propertyCodec2.PropertyInfo.Name);
                    stringBuilder.Append("=");
                    stringBuilder.Append(propertyCodec2.PropertyInfo.GetValue(instance, BindingFlags.Default, null, null, null));
                    stringBuilder.Append("\n");
                }

                throw new Exception("Struct decode failed; Type: " + instance.GetType().Name + " decodedPropertis: " + stringBuilder, innerException);
            }
        }

        public override string ToString() => string.Concat("StructCodec[", type, "]");
    }
}