using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [SerialVersionUID(1505906670608L)]
    [Shared]
    public class ForceFieldTranformComponent : Component {
        public Movement Movement { get; set; }
    }
}