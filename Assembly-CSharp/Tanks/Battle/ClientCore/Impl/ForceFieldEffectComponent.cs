using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1503314709814L)]
    public class ForceFieldEffectComponent : Component {
        public bool OwnerTeamCanShootThrough { get; set; }
    }
}