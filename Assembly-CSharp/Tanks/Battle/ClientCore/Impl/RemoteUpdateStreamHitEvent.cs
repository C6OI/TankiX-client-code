using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1430210607200L)]
    public class RemoteUpdateStreamHitEvent : BaseUpdateStreamHitEvent { }
}