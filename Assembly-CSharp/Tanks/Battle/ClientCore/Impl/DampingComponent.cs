using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1437725485852L)]
    public class DampingComponent : Component {
        public float Damping { get; set; }
    }
}