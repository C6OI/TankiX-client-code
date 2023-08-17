using Platform.Library.ClientProtocol.Impl;

namespace Platform.Library.ClientProtocol.API {
    public interface ClientProtocolInstancesCache {
        ProtocolBuffer GetProtocolBufferInstance();

        void ReleaseProtocolBufferInstance(ProtocolBuffer protocolBuffer);

        MemoryStreamData GetMemoryStreamDataInstance();

        void ReleaseMemoryStreamData(MemoryStreamData memoryStreamData);
    }
}