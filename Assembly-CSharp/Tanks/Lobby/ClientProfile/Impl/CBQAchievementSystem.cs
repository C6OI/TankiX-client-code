using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class CBQAchievementSystem : ECSSystem {
        [OnEventFire]
        public void ShowCBQBadge(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen, CBQUserNode selfUser) {
            homeScreen.component.CbqBadge.SetActive(true);
        }

        public class CBQUserNode : Node {
            public ClosedBetaQuestAchievementComponent closedBetaQuestAchievement;
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }
    }
}