using System.Collections.Generic;
using log4net;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class CommandsSender : FlowListener {
        readonly CommandCollector commandCollector;

        readonly SharedEntityRegistry entityRegistry;

        readonly NetworkService networkService;

        readonly ILog logger;

        public CommandsSender(EngineService engineService, NetworkService networkService, ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry) {
            this.networkService = networkService;
            this.entityRegistry = entityRegistry;
            commandCollector = new CommandCollector();
            logger = LoggerProvider.GetLogger(this);
            EventCommandCollector eventListener = new(commandCollector, componentAndEventRegistrator, entityRegistry);
            ComponentCommandCollector componentListener = new(commandCollector, componentAndEventRegistrator, entityRegistry);
            engineService.AddFlowListener(this);
            engineService.AddComponentListener(componentListener);
            engineService.AddEventListener(eventListener);
        }

        [Inject] public static ClientNetworkInstancesCache ClientNetworkInstancesCache { get; set; }

        public void OnFlowFinish() {
            List<Command> commands = commandCollector.Commands;

            if (commands.Count > 0) {
                List<Command> commandCollection = ClientNetworkInstancesCache.GetCommandCollection();
                int count = commands.Count;

                for (int i = 0; i < count; i++) {
                    Command command = commands[i];
                    logger.InfoFormat("Out {0}", command);
                    commandCollection.Add(command);
                }

                if (commandCollection.Count > 0) {
                    CommandPacket commandPacketInstance = ClientNetworkInstancesCache.GetCommandPacketInstance(commandCollection);
                    networkService.SendCommandPacket(commandPacketInstance);
                }

                commandCollector.Clear();
            }
        }

        public void OnFlowClean() { }
    }
}