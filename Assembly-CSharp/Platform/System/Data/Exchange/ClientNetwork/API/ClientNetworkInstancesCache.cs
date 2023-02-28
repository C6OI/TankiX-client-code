using System.Collections.Generic;
using Platform.System.Data.Exchange.ClientNetwork.Impl;

namespace Platform.System.Data.Exchange.ClientNetwork.API {
    public interface ClientNetworkInstancesCache {
        List<Command> GetCommandCollection();

        void ReleaseCommandCollection(List<Command> commands);

        CommandPacket GetCommandPacketInstance(List<Command> commands);

        void ReleaseCommandPacketWithCommandsCollection(CommandPacket commandPacket);
    }
}