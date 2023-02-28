using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class LoadUserComponent : Component {
        public LoadUserComponent(long userId) => UserId = userId;

        public long UserId { get; }
    }
}