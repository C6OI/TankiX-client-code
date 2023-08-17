using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class UserXCrystalsIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void Sync(UserXCrystalsChangedEvent e, SelfUserMoneyNode money,
            [JoinAll] [Combine] SingleNode<UserXCrystalsIndicatorComponent> indicator) =>
            indicator.component.SetMoneyAnimated(money.userXCrystals.Money);

        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfUserMoneyNode money,
            [Combine] SingleNode<UserXCrystalsIndicatorComponent> indicator) =>
            indicator.component.SetMoneyImmediately(money.userXCrystals.Money);

        public class SelfUserMoneyNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserXCrystalsComponent userXCrystals;
        }
    }
}