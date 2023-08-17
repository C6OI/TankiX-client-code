using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1469531017818L)]
    public class SerachUserIdByUidResultEvent : Event {
        public bool Found { get; set; }

        public long UserId { get; set; }
    }
}