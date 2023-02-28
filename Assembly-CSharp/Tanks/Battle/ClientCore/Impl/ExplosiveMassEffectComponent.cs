using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1543402751411L)]
    public class ExplosiveMassEffectComponent : Component {
        public float Radius { get; set; }

        public long Delay { get; set; }
    }
}