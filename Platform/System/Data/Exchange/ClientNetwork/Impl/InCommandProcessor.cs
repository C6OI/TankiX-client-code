using System.Collections.Generic;
using log4net;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientLogger.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class InCommandProcessor {
        readonly EngineServiceInternal engineService;

        readonly List<Command> incomeCommand;

        readonly ILog log;

        public InCommandProcessor(EngineServiceInternal engineService, NetworkService networkService) {
            incomeCommand = new List<Command>();
            this.engineService = engineService;
            log = LoggerProvider.GetLogger(this);
            networkService.OnCommandPacketReceived += OnCommandPacketReceived;
        }

        public void OnCommandPacketReceived(CommandPacket packet) {
            incomeCommand.AddRange(packet.Commands);
            ExecutePacket();
        }

        void ExecutePacket() {
            while (true) {
                int flowBoundPosition = FindFlowBoundPosition();

                if (flowBoundPosition < 0) {
                    break;
                }

                Flow flow = engineService.NewFlow();
                flow.SkipLogError = true;

                flow.StartWith(delegate {
                    try {
                        for (int i = 0; i <= flowBoundPosition; i++) {
                            Command command = incomeCommand[i];
                            log.InfoFormat("Execute {0}", command);
                            flow.ScheduleWith(command.Execute);
                        }
                    } finally {
                        incomeCommand.RemoveRange(0, flowBoundPosition + 1);
                    }
                });

                engineService.ExecuteFlow(flow);
            }
        }

        int FindFlowBoundPosition() {
            for (int i = 0; i < incomeCommand.Count; i++) {
                Command command = incomeCommand[i];

                if (command.GetType() == typeof(FlowBoundCommand)) {
                    return i;
                }
            }

            return -1;
        }
    }
}