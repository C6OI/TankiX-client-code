using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(-3961778961585441606L)]
    public class BonusRegionComponent : Component {
        public BonusType Type { get; set; }
    }
}