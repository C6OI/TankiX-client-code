using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.Impl {
    public class ProfileScreenContextComponent : Component {
        public ProfileScreenContextComponent() { }

        public ProfileScreenContextComponent(long userId) => UserId = userId;

        public long UserId { get; set; }
    }
}