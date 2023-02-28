using System;
using Platform.Library.ClientProtocol.Impl;

namespace Platform.Library.ClientProtocol.API {
    public abstract class NotOptionalCodec : Codec {
        public abstract void Init(Protocol protocol);

        public virtual void Encode(ProtocolBuffer protocolBuffer, object data) {
            if (data == null) {
                throw new OptionalAnnotationNotFoundForNullObjectException();
            }
        }

        public abstract object Decode(ProtocolBuffer protocolBuffer);

        public virtual void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance) {
            throw new NotImplementedException();
        }
    }
}