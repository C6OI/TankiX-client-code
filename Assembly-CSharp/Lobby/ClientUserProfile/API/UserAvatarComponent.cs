using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1545809085571L)]
    public class UserAvatarComponent : Component {
        public UserAvatarComponent() { }

        public UserAvatarComponent(string id) => Id = id;

        public string Id { get; set; }
    }
}