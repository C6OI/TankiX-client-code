using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [SerialVersionUID(1469526368502L)]
    [Shared]
    public class SearchUserIdByUidEvent : Event {
        public string Uid { get; set; }
    }
}