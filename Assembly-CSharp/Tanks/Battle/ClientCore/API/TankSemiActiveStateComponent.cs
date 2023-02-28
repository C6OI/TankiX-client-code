using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.API {
    [Shared]
    [SerialVersionUID(5166099393636831290L)]
    public class TankSemiActiveStateComponent : Component {
        public int ActivationTime { get; set; }
    }
}