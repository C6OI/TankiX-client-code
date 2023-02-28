using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class UserXCrystalsIndicatorSystem : ECSSystem {
        [OnEventFire]
        public void Sync(UserXCrystalsChangedEvent e, SelfUserMoneyNode money, [JoinAll] [Combine] SingleNode<UserXCrystalsIndicatorComponent> indicator) {
            indicator.component.SetMoneyAnimated(money.userXCrystals.Money);
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfUserMoneyNode money, [Combine] SingleNode<UserXCrystalsIndicatorComponent> indicator) {
            indicator.component.SetMoneyImmediately(money.userXCrystals.Money);
        }

        [OnEventFire]
        public void Sync(UserXCrystalsChangedEvent e, SelfUserMoneyNode money, [JoinAll] [Combine] XCrystalsIndicatorNode indicator) {
            indicator.xCrystalsIndicator.Value = money.userXCrystals.Money;
        }

        [OnEventFire]
        public void SyncWithBuffer(UserXCrystalsChangedEvent e, UserMoneyBufferNode money, [JoinAll] [Combine] XCrystalsIndicatorWithBufferNode indicator) {
            indicator.xCrystalsIndicator.Value = money.userXCrystals.Money - money.userMoneyBuffer.XCrystalBuffer;
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfUserMoneyNode money, [Combine] XCrystalsIndicatorNode indicator) {
            indicator.xCrystalsIndicator.Value = money.userXCrystals.Money;
        }

        [OnEventFire]
        public void InitWithBuffer(NodeAddedEvent e, UserMoneyBufferNode money, [Combine] XCrystalsIndicatorWithBufferNode indicator) {
            indicator.xCrystalsIndicator.Value = money.userXCrystals.Money - money.userMoneyBuffer.XCrystalBuffer;
        }

        [OnEventFire]
        public void Sync(UserMoneyChangedEvent e, SelfUserMoneyNode money, [JoinAll] [Combine] CrystalsIndicatorNode indicator) {
            indicator.crystalsIndicator.Value = money.userMoney.Money;
        }

        [OnEventFire]
        public void SyncWithBuffer(UserMoneyChangedEvent e, UserMoneyBufferNode money, [JoinAll] [Combine] CrystalsIndicatorWithBufferNode indicator) {
            indicator.crystalsIndicator.Value = money.userMoney.Money - money.userMoneyBuffer.CrystalBuffer;
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfUserMoneyNode money, [Combine] CrystalsIndicatorNode indicator) {
            indicator.crystalsIndicator.Value = money.userMoney.Money;
        }

        [OnEventFire]
        public void InitWithBuffer(NodeAddedEvent e, UserMoneyBufferNode money, [Combine] CrystalsIndicatorWithBufferNode indicator) {
            indicator.crystalsIndicator.Value = money.userMoney.Money - money.userMoneyBuffer.CrystalBuffer;
        }

        public class SelfUserMoneyNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;

            public UserMoneyComponent userMoney;

            public UserXCrystalsComponent userXCrystals;
        }

        public class UserMoneyBufferNode : SelfUserMoneyNode {
            public UserMoneyBufferComponent userMoneyBuffer;
        }

        public class CrystalsIndicatorWithBufferNode : Node {
            public BufferComponent buffer;
            public CrystalsIndicatorComponent crystalsIndicator;
        }

        public class XCrystalsIndicatorWithBufferNode : Node {
            public BufferComponent buffer;
            public XCrystalsIndicatorComponent xCrystalsIndicator;
        }

        [Not(typeof(BufferComponent))]
        public class CrystalsIndicatorNode : Node {
            public CrystalsIndicatorComponent crystalsIndicator;
        }

        [Not(typeof(BufferComponent))]
        public class XCrystalsIndicatorNode : Node {
            public XCrystalsIndicatorComponent xCrystalsIndicator;
        }
    }
}