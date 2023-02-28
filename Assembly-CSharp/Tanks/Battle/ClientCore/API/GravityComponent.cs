using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [SerialVersionUID(1435652501758L)]
    [Shared]
    public class GravityComponent : Component {
        public float Gravity { get; set; }

        public GravityType GravityType { get; set; }
    }
}