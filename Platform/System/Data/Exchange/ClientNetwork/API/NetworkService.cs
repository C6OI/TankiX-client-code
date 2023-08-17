using System;
using Platform.System.Data.Exchange.ClientNetwork.Impl;

namespace Platform.System.Data.Exchange.ClientNetwork.API {
    public interface NetworkService {
        bool Connected { get; }

        event Action<CommandPacket> OnCommandPacketReceived;

        void Connect(string host, int[] ports);

        void SendCommandPacket(CommandPacket packet);

        void Disconnect();

        void ProcessNetworkTasks();
    }
}