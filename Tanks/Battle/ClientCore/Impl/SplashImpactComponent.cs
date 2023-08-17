using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1438773081827L)]
    public class SplashImpactComponent : Component {
        public float ImpactForce { get; set; }
    }
}