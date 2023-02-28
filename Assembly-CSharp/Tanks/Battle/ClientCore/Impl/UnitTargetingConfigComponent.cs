using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636364931473899150L)]
    public class UnitTargetingConfigComponent : Component {
        public float TargetingPeriod { get; set; }

        public float WorkDistance { get; set; }
    }
}