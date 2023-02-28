using System;
using Platform.System.Data.Exchange.ClientNetwork.Impl;

namespace Platform.System.Data.Exchange.ClientNetwork.API {
    public interface NetworkService {
        bool IsConnected { get; }

        bool IsDecodeState { get; }

        bool SkipThrowOnCommandExecuteException { get; set; }

        bool SplitShareCommand { set; }

        event Action<Command, Exception> OnCommandExecuteException;

        void ConnectAsync(string host, int port, Action completeCallback);

        void ReadAndExecuteCommands(long networkSliceTime = 0L, long networkMaxDelayTime = 0L);

        void WriteCommands();

        void SendCommandPacket(CommandPacket packet);

        void Disconnect();
    }
}