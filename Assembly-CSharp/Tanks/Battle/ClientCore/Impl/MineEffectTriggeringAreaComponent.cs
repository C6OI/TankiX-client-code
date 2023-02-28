using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(636377093029435859L)]
    public class MineEffectTriggeringAreaComponent : Component {
        public float Radius { get; set; }
    }
}