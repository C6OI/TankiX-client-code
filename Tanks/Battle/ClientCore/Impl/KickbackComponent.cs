using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(1437989437781L)]
    public class KickbackComponent : Component {
        public float KickbackForce { get; set; }
    }
}