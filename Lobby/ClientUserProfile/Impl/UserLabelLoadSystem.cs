using Lobby.ClientEntrance.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.Impl {
    public class UserLabelLoadSystem : ECSSystem {
        [OnEventFire]
        public void AttachUser(NodeAddedEvent e, UserLabelLoadedNode userLabel) =>
            userLabel.Entity.AddComponent(new UserGroupComponent(userLabel.userLabel.UserId));

        public class UserLabelLoadedNode : Node {
            public LoadUserComponent loadUser;
            public UserLabelComponent userLabel;

            public UserLoadedComponent userLoaded;
        }
    }
}