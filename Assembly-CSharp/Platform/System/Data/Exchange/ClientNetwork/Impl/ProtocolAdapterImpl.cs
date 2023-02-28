using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientProtocol.API;
using Platform.Library.ClientProtocol.Impl;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class ProtocolAdapterImpl : ProtocolAdapter {
        static readonly Component[] EmptyComponents = new Component[0];

        readonly CommandsCodec commandsCodec;

        MemoryStreamData partialReceivedData;
        readonly Protocol protocol;

        public ProtocolAdapterImpl(Protocol protocol, CommandsCodec commandsCodec) {
            this.protocol = protocol;
            this.commandsCodec = commandsCodec;
            partialReceivedData = ClientProtocolInstancesCache.GetMemoryStreamDataInstance();
        }

        [Inject] public static ClientProtocolInstancesCache ClientProtocolInstancesCache { get; set; }

        [Inject] public static ClientNetworkInstancesCache ClientNetworkInstancesCache { get; set; }

        [Inject] public static ProtocolFlowInstancesCache ProtocolFlowInstances { get; set; }

        public bool SplitShareCommand { get; set; }

        public MemoryStreamData Encode(CommandPacket commandPacket) {
            MemoryStreamData memoryStreamDataInstance = ClientProtocolInstancesCache.GetMemoryStreamDataInstance();
            ProtocolBuffer protocolBufferInstance = ClientProtocolInstancesCache.GetProtocolBufferInstance();
            List<Command> commands = commandPacket.Commands;
            int count = commands.Count;

            for (int i = 0; i < count; i++) {
                commandsCodec.Encode(protocolBufferInstance, commands[i]);
            }

            protocol.WrapPacket(protocolBufferInstance, memoryStreamDataInstance);
            ClientProtocolInstancesCache.ReleaseProtocolBufferInstance(protocolBufferInstance);
            return memoryStreamDataInstance;
        }

        public void AddChunk(byte[] chunk, int length) {
            partialReceivedData.Write(chunk, 0, length);
        }

        public void FinalizeDecodedCommandPacket(CommandPacket commandPacket) {
            ClientNetworkInstancesCache.ReleaseCommandPacketWithCommandsCollection(commandPacket);
        }

        public bool TryDecode(out CommandPacket commandPacket) {
            commandPacket = null;

            if (partialReceivedData.Length == 0) {
                return false;
            }

            ProtocolBuffer protocolBufferInstance = ClientProtocolInstancesCache.GetProtocolBufferInstance();
            bool flag = TryUnwrapPacket(protocolBufferInstance);

            if (flag) {
                KeepPartialReceivedData();
                commandPacket = DecodePacket(protocolBufferInstance);
            }

            ClientProtocolInstancesCache.ReleaseProtocolBufferInstance(protocolBufferInstance);
            return flag;
        }

        bool TryUnwrapPacket(ProtocolBuffer packetData) {
            long position = partialReceivedData.Position;
            partialReceivedData.Position = 0L;

            if (!protocol.UnwrapPacket(partialReceivedData, packetData)) {
                partialReceivedData.Position = position;
                return false;
            }

            return true;
        }

        void KeepPartialReceivedData() {
            if (IsPartialDataEmpty()) {
                partialReceivedData.Position = 0L;
                partialReceivedData.SetLength(0L);
                return;
            }

            long position = partialReceivedData.Position;
            long length = partialReceivedData.Length;
            MemoryStreamData memoryStreamData = partialReceivedData;
            partialReceivedData = ClientProtocolInstancesCache.GetMemoryStreamDataInstance();
            partialReceivedData.Write(memoryStreamData.GetBuffer(), (int)position, (int)(length - position));
            ClientProtocolInstancesCache.ReleaseMemoryStreamData(memoryStreamData);
        }

        CommandPacket DecodePacket(ProtocolBuffer packetData) {
            List<Command> commandCollection = ClientNetworkInstancesCache.GetCommandCollection();

            while (packetData.Data.Position < packetData.Data.Length) {
                Command command = (Command)commandsCodec.Decode(packetData);

                if (command.GetType() == typeof(InitTimeCommand) || command.GetType() == typeof(CloseCommand)) {
                    command.Execute(null);
                } else if (!TrySplitCommands(command, commandCollection)) {
                    commandCollection.Add(command);
                }
            }

            return ClientNetworkInstancesCache.GetCommandPacketInstance(commandCollection);
        }

        bool TrySplitCommands(Command command, List<Command> commands) {
            if (!SplitShareCommand) {
                return false;
            }

            EntityShareCommand entityShareCommand = command as EntityShareCommand;

            if (entityShareCommand == null) {
                return false;
            }

            Component[] components = entityShareCommand.Components;
            entityShareCommand.Components = EmptyComponents;
            commands.Add(entityShareCommand);
            Component[] array = components;

            foreach (Component component in array) {
                ComponentAddCommand instance = ProtocolFlowInstances.GetInstance<ComponentAddCommand>();
                instance.Entity = entityShareCommand.GetOrCreateEntity();
                instance.Component = component;
                commands.Add(instance);
            }

            return true;
        }

        bool IsPartialDataEmpty() => partialReceivedData.Length == partialReceivedData.Position;
    }
}