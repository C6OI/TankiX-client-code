using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(-1910863908782544246L)]
    public class FlagDropRequestEvent : Event { }
}