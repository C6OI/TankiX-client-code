using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(8420700272384380156L)]
    [Shared]
    public class HealthConfigComponent : Component {
        public float BaseHealth { get; set; }
    }
}