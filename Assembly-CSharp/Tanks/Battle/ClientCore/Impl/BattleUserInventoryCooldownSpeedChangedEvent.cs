using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636366611234585965L)]
    public class BattleUserInventoryCooldownSpeedChangedEvent : Event { }
}