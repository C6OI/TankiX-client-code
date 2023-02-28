using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientFriends.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    public class DisplayProfileScreenHeaderSystem : ECSSystem {
        [OnEventFire]
        public void SetMyProfileHeader(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] SelfUserNode selfUser) {
            profileScreen.Entity.AddComponent(new ScreenHeaderTextComponent(profileScreen.profileScreenLocalization.MyProfileHeaderText));
        }

        [OnEventFire]
        public void SetFriendProfileHeader(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [Context] [JoinByUser] FriendUserNode friendUser) {
            SetScreenHeaderEvent setScreenHeaderEvent = new();
            setScreenHeaderEvent.Immediate(profileScreen.profileScreenLocalization.FriendsProfileHeaderText);
            ScheduleEvent(setScreenHeaderEvent, profileScreen);
        }

        [OnEventFire]
        public void SetNotFriendProfileHeader(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] NotFriendUserNode notFriendUser) {
            SetScreenHeaderEvent setScreenHeaderEvent = new();
            setScreenHeaderEvent.Immediate(profileScreen.profileScreenLocalization.ProfileHeaderText);
            ScheduleEvent<SetScreenHeaderEvent>(profileScreen);
        }

        [OnEventFire]
        public void SetNotFriendProfileHeader(NodeRemoveEvent e, FriendUserNode friendUser, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen) {
            SetScreenHeaderEvent setScreenHeaderEvent = new();
            setScreenHeaderEvent.Immediate(profileScreen.profileScreenLocalization.ProfileHeaderText);
            ScheduleEvent(setScreenHeaderEvent, profileScreen);
        }

        public class ProfileScreenWithUserGroupNode : Node {
            public ActiveScreenComponent activeScreen;
            public ProfileScreenComponent profileScreen;

            public ProfileScreenLocalizationComponent profileScreenLocalization;

            public UserGroupComponent userGroup;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class FriendUserNode : Node {
            public AcceptedFriendComponent acceptedFriend;

            public UserGroupComponent userGroup;
        }

        [Not(typeof(AcceptedFriendComponent))]
        [Not(typeof(SelfUserComponent))]
        public class NotFriendUserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }
    }
}