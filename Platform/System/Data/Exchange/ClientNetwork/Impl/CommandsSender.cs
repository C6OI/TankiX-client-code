using System.Collections.Generic;
using log4net;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class CommandsSender {
        readonly CommandCollector commandCollector;

        readonly SharedEntityRegistry entityRegistry;

        readonly ILog logger;

        readonly NetworkService networkService;

        public CommandsSender(EngineService engineService, NetworkService networkService,
            ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry) {
            this.networkService = networkService;
            this.entityRegistry = entityRegistry;
            commandCollector = new CommandCollector();
            logger = LoggerProvider.GetLogger(this);

            engineService.RegisterSystem(new EventCommandCollectorSystem(commandCollector,
                componentAndEventRegistrator,
                entityRegistry));

            engineService.RegisterSystem(
                new ComponentCommandCollectorSystem(commandCollector, componentAndEventRegistrator, entityRegistry));

            engineService.FlowFinishEvents += OnFinishFlow;
        }

        [Inject] public static ClientNetworkInstancesCache ClientNetworkInstancesCache { get; set; }

        void OnFinishFlow(Flow flow) {
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
                    CommandPacket commandPacketInstance =
                        ClientNetworkInstancesCache.GetCommandPacketInstance(commandCollection);

                    networkService.SendCommandPacket(commandPacketInstance);
                }

                commandCollector.Clear();
            }
        }
    }
}