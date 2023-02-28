using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486559652910L)]
    public class TemperatureEffectComponent : Component {
        public float Factor { get; set; }
    }
}