using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(6258344835131144773L)]
    public class TeamColorComponent : Component {
        public TeamColor TeamColor { get; set; }
    }
}