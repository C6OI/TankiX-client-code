using Platform.Library.ClientProtocol.Impl;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public interface ProtocolAdapter {
        MemoryStreamData Encode(CommandPacket packet);

        void AddChunk(byte[] chunk, int length);

        bool TryDecode(out CommandPacket packet);

        void FinalizeDecodedCommandPacket(CommandPacket commandPacket);
    }
}