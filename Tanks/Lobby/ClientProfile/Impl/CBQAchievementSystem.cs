using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class CBQAchievementSystem : ECSSystem {
        [OnEventFire]
        public void ShowCBQBadge(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen, CBQUserNode selfUser) =>
            homeScreen.component.CbqBadge.SetActive(true);

        public class CBQUserNode : Node {
            public ClosedBetaQuestAchievementComponent closedBetaQuestAchievement;
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }
    }
}