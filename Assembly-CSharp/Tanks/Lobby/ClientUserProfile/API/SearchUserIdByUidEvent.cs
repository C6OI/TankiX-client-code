using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1469526368502L)]
    public class SearchUserIdByUidEvent : Event {
        public string Uid { get; set; }
    }
}