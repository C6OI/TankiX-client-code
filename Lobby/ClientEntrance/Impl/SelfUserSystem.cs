using Lobby.ClientEntrance.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientEntrance.Impl {
    public class SelfUserSystem : ECSSystem {
        [OnEventFire]
        public void MarkSelfUser(NodeAddedEvent e, UserNode user, [JoinByUser] [Context] ClientSessionNode clientSession) {
            user.Entity.AddComponent<SelfUserComponent>();
            user.Entity.AddComponent<SelfComponent>();
        }

        [OnEventFire]
        public void RemoveSelfUser(NodeRemoveEvent e, UserNode user, [JoinByUser] ClientSessionNode clientSession) =>
            throw new WhiteWalkerException();

        [OnEventFire]
        public void DoNothing(NodeAddedEvent e, SingleNode<LobbyComponent> n) { }

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class ClientSessionNode : Node {
            public ClientSessionComponent clientSession;
            public UserGroupComponent userGroup;
        }
    }
}