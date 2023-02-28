using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1437983715951L)]
    public class ShaftAimingImpactComponent : Component {
        public float MaxImpactForce { get; set; }
    }
}