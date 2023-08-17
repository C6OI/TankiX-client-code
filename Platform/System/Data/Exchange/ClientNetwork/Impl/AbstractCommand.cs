using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public abstract class AbstractCommand : Command {
        public abstract void Execute(Engine engine);
    }
}