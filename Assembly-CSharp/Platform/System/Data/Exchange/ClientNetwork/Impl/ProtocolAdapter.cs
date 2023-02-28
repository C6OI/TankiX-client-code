using Platform.Library.ClientProtocol.Impl;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public interface ProtocolAdapter {
        bool SplitShareCommand { get; set; }

        MemoryStreamData Encode(CommandPacket packet);

        void AddChunk(byte[] chunk, int length);

        bool TryDecode(out CommandPacket packet);

        void FinalizeDecodedCommandPacket(CommandPacket commandPacket);
    }
}