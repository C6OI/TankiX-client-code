using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.Impl {
    public class ProfileScreenLoadSystem : ECSSystem {
        [OnEventFire]
        public void AttachSelfProfileScreenOrLoadRemoteUserProfile(NodeAddedEvent e, ProfileScreenNode profileScreen,
            [JoinByScreen] ProfileScreenContextNode profileScreenContext, [JoinAll] SelfUserNode selfUser) {
            if (profileScreenContext.profileScreenContext.UserId.Equals(selfUser.Entity.Id)) {
                selfUser.userGroup.Attach(profileScreen.Entity);
            } else {
                ScheduleEvent(new RequestLoadUserProfileEvent(profileScreenContext.profileScreenContext.UserId), selfUser);
            }
        }

        [OnEventFire]
        public void AttachProfileScreenToUserGroup(UserProfileLoadedEvent e, RemoteUserNode remoteUser,
            [JoinAll] ProfileScreenNode screen, [JoinByScreen] ProfileScreenContextNode profileScreenContext) {
            if (remoteUser.Entity.Id.Equals(profileScreenContext.profileScreenContext.UserId)) {
                remoteUser.userGroup.Attach(screen.Entity);
            }
        }

        [OnEventFire]
        public void SendRequestUnloadUserProfile(NodeRemoveEvent e, ProfileScreenContextNode context,
            [JoinByScreen] ProfileScreenNode profileScreen, [JoinAll] SelfUserNode selfUser) {
            if (!context.profileScreenContext.UserId.Equals(selfUser.Entity.Id)) {
                ScheduleEvent(new RequestUnloadUserProfileEvent(context.profileScreenContext.UserId), selfUser);
            }
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(SelfUserComponent))]
        public class RemoteUserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class ProfileScreenNode : Node {
            public ProfileScreenComponent profileScreen;

            public ScreenGroupComponent screenGroup;
        }

        public class ProfileScreenContextNode : Node {
            public ProfileScreenContextComponent profileScreenContext;
            public ScreenGroupComponent screenGroup;
        }
    }
}