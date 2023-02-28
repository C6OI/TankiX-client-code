using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(1482920154068L)]
    public class UserSubscribeComponent : Component {
        public bool Subscribed { get; set; }
    }
}