using System;
using System.Collections;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class DictionaryCodec : Codec {
        readonly CodecInfoWithAttributes keyRequest;
        readonly Type type;

        readonly CodecInfoWithAttributes valueRequest;

        Codec keyCodec;

        Codec valueCodec;

        public DictionaryCodec(Type type, CodecInfoWithAttributes keyRequest, CodecInfoWithAttributes valueRequest) {
            this.type = type;
            this.keyRequest = keyRequest;
            this.valueRequest = valueRequest;
        }

        public void Init(Protocol protocol) {
            keyCodec = protocol.GetCodec(keyRequest);
            valueCodec = protocol.GetCodec(valueRequest);
        }

        public void Encode(ProtocolBuffer protocolBuffer, object data) {
            IDictionary dictionary = (IDictionary)data;
            LengthCodecHelper.EncodeLength(protocolBuffer.Data.Stream, dictionary.Count);

            if (dictionary.Count <= 0) {
                return;
            }

            foreach (object key in dictionary.Keys) {
                keyCodec.Encode(protocolBuffer, key);
                valueCodec.Encode(protocolBuffer, dictionary[key]);
            }
        }

        public object Decode(ProtocolBuffer protocolBuffer) {
            int num = LengthCodecHelper.DecodeLength(protocolBuffer.Reader);
            IDictionary dictionary = (IDictionary)Activator.CreateInstance(type, null);

            if (num > 0) {
                for (int i = 0; i < num; i++) {
                    object key = keyCodec.Decode(protocolBuffer);
                    object value = valueCodec.Decode(protocolBuffer);
                    dictionary.Add(key, value);
                }
            }

            return dictionary;
        }
    }
}