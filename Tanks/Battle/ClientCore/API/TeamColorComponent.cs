using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(6258344835131144773L)]
    [Shared]
    public class TeamColorComponent : Component {
        public TeamColor TeamColor { get; set; }
    }
}