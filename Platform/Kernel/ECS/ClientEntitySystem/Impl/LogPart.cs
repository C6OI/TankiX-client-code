using Platform.Library.ClientDataStructures.API;

namespace Platform.Kernel.ECS.ClientEntitySystem.Impl {
    public interface LogPart {
        Optional<string> GetSkipReason();
    }
}