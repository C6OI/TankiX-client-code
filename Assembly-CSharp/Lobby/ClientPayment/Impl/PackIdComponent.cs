using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientPayment.Impl {
    [Shared]
    [SerialVersionUID(636185325862880582L)]
    public class PackIdComponent : Component {
        public long Id { get; set; }
    }
}