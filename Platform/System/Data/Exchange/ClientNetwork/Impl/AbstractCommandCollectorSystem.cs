using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public class AbstractCommandCollectorSystem : ECSSystem {
        readonly CommandCollector commandCollector;

        public AbstractCommandCollectorSystem(CommandCollector commandCollector) => this.commandCollector = commandCollector;

        protected void AddCommand(Command command) => commandCollector.Add(command);
    }
}