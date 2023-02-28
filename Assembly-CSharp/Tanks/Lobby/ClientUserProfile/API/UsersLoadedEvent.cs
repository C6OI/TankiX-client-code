using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1458555361768L)]
    public class UsersLoadedEvent : Event {
        public long RequestEntityId { get; set; }
    }
}