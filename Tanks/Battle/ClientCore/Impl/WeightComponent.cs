using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1437571863912L)]
    [Shared]
    public class WeightComponent : Component {
        public float Weight { get; set; }
    }
}