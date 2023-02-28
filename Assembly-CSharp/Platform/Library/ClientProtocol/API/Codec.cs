namespace Platform.Library.ClientProtocol.API {
    public interface Codec {
        void Init(Protocol protocol);

        void Encode(ProtocolBuffer protocolBuffer, object data);

        object Decode(ProtocolBuffer protocolBuffer);

        void DecodeToInstance(ProtocolBuffer protocolBuffer, object instance);
    }
}