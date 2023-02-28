using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientProfile.API;

namespace Tanks.Lobby.ClientProfile.Impl {
    public class CrystalsBufferSystem : ECSSystem {
        [OnEventFire]
        public void AddMoneyBuffer(NodeAddedEvent e, SelfUserNode user) {
            user.Entity.AddComponent<UserMoneyBufferComponent>();
        }

        [OnEventFire]
        public void ChangeBufferBy(ChangeUserMoneyBufferEvent e, UserMoneyBufferNode buffer) {
            buffer.userMoneyBuffer.ChangeCrystalBufferBy(e.Crystals);
            buffer.userMoneyBuffer.ChangeXCrystalBufferBy(e.XCrystals);
            ScheduleEvent<UserMoneyChangedEvent>(buffer);
            ScheduleEvent<UserXCrystalsChangedEvent>(buffer);
        }

        [OnEventFire]
        public void ResetBuffer(ResetUserMoneyBufferEvent e, UserMoneyBufferNode buffer) {
            buffer.userMoneyBuffer.CrystalBuffer = 0;
            buffer.userMoneyBuffer.XCrystalBuffer = 0;
        }

        public class SelfUserNode : Node {
            public SelfComponent self;
            public UserComponent user;
        }

        public class UserMoneyBufferNode : SelfUserNode {
            public UserMoneyBufferComponent userMoneyBuffer;
        }
    }
}