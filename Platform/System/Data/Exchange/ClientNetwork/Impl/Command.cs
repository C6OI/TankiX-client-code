using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Platform.System.Data.Exchange.ClientNetwork.Impl {
    public interface Command {
        void Execute(Engine engine);
    }
}