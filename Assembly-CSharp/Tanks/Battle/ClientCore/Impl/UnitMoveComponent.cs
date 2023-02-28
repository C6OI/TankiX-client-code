using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1485519196443L)]
    [Shared]
    public class UnitMoveComponent : Component {
        public Movement Movement { get; set; }
    }
}