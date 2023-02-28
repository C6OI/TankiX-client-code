using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1486455226183L)]
    [Shared]
    public class UnitTargetComponent : Component {
        public Entity Target { get; set; }

        public Entity TargetIncarnation { get; set; }
    }
}