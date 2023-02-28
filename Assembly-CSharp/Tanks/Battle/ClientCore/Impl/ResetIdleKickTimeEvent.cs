using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1448342129940L)]
    [Shared]
    public class ResetIdleKickTimeEvent : Event { }
}