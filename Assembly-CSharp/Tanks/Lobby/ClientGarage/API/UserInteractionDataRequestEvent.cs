using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1454623211245L)]
    public class UserInteractionDataRequestEvent : Event {
        public long UserId { get; set; }
    }
}