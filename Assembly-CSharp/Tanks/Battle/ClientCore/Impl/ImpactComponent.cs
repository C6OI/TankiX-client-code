using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1437983636148L)]
    public class ImpactComponent : Component {
        public float ImpactForce { get; set; }
    }
}