using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientEntrance.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class EntranceSystem : ECSSystem {
        [OnEventFire]
        public void SendUserOnlineEvent(NodeAddedEvent e, UserOnlineNode userOnline, [JoinAll] SessionNode session) {
            ScheduleEvent<UserOnlineEvent>(session);
        }

        [OnEventFire]
        public void SendUserQuestReadyEvent(NodeAddedEvent e, UserQuestReadyNode userNode, [JoinAll] SessionNode session) {
            ScheduleEvent<UserQuestReadyEvent>(session);
        }

        public class UserOnlineNode : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserOnlineComponent userOnline;
        }

        public class UserQuestReadyNode : UserOnlineNode {
            public QuestReadyComponent questReady;
        }

        public class SessionNode : Node {
            public ClientSessionComponent clientSession;
        }
    }
}