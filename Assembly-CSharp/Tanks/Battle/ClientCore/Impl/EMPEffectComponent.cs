using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636250000933021510L)]
    public class EMPEffectComponent : Component {
        public float Radius { get; set; }
    }
}