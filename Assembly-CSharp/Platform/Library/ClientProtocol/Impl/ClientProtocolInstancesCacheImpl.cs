using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientDataStructures.Impl.Cache;
using Platform.Library.ClientProtocol.API;

namespace Platform.Library.ClientProtocol.Impl {
    public class ClientProtocolInstancesCacheImpl : ClientProtocolInstancesCache {
        readonly Cache<MemoryStreamData> memoryStreamDataCache;
        readonly Cache<ProtocolBuffer> protocolBufferCache;

        public ClientProtocolInstancesCacheImpl() {
            protocolBufferCache = new CacheImpl<ProtocolBuffer>(delegate(ProtocolBuffer a) {
                a.Clear();
            });

            memoryStreamDataCache = new CacheImpl<MemoryStreamData>(delegate(MemoryStreamData a) {
                a.Clear();
            });
        }

        public ProtocolBuffer GetProtocolBufferInstance() => protocolBufferCache.GetInstance();

        public void ReleaseProtocolBufferInstance(ProtocolBuffer protocolBuffer) {
            protocolBufferCache.Free(protocolBuffer);
        }

        public MemoryStreamData GetMemoryStreamDataInstance() => memoryStreamDataCache.GetInstance();

        public void ReleaseMemoryStreamData(MemoryStreamData memoryStreamData) {
            memoryStreamDataCache.Free(memoryStreamData);
        }
    }
}