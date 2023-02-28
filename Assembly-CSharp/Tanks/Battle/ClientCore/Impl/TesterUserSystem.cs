using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class TesterUserSystem : ElevatedAccessUserBaseSystem {
        [OnEventFire]
        public void InitTesterConsole(NodeAddedEvent e, TesterUserNode tester) { }

        void InitSquadTester(string parameters) {
            ScheduleEvent(new InitSquadTesterEvent(), user);
        }

        public class TesterUserNode : SelfUserNode {
            public UserTesterComponent userTester;
        }
    }
}