using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636366605665347423L)]
    public class BattleUserInventoryCooldownSpeedComponent : Component {
        public float SpeedCoeff { get; set; }
    }
}