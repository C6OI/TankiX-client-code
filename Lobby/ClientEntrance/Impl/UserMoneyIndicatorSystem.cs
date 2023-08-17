using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientEntrance.Impl {
    public class UserMoneyIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfUserMoneyNode userMoney,
            [Combine] UserMoneyIndicatorNode userMoneyIndicator) =>
            userMoneyIndicator.userMoneyIndicator.SetMoneyImmediately(userMoney.userMoney.Money);

        [OnEventFire]
        public void UpdateIndicator(UserMoneyChangedEvent e, SelfUserMoneyNode userMoney,
            [JoinAll] [Combine] UserMoneyIndicatorNode userMoneyIndicator) =>
            userMoneyIndicator.userMoneyIndicator.SetMoneyAnimated(userMoney.userMoney.Money);

        public class SelfUserMoneyNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserMoneyComponent userMoney;
        }

        public class UserMoneyIndicatorNode : Node {
            public UserMoneyIndicatorComponent userMoneyIndicator;
        }
    }
}