using Lobby.ClientCommunicator.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatSystem : ECSSystem {
        [OnEventFire]
        public void AddGeneralChatToScreenGroup(NodeAddedEvent e, SelfUserNode userNode, GeneralBattleChatNode chatNode,
            ChatHUDNode chatHUDNode) => chatHUDNode.screenGroup.Attach(chatNode.Entity);

        [OnEventFire]
        public void AddTeamChatToScreenGroup(NodeAddedEvent e, SelfUserNode userNode, TeamBattleChatNode chatNode,
            ChatHUDNode chatHUDNode) => chatHUDNode.screenGroup.Attach(chatNode.Entity);

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;
        }

        public class ChatHUDNode : Node {
            public ChatHUDComponent chatHUD;

            public ScreenGroupComponent screenGroup;
        }

        [Not(typeof(ScreenGroupComponent))]
        public class ChatNode : Node {
            public ChatComponent chat;
        }

        public class GeneralBattleChatNode : ChatNode {
            public GeneralBattleChatComponent generalBattleChat;
        }

        public class TeamBattleChatNode : ChatNode {
            public TeamBattleChatComponent teamBattleChat;
        }
    }
}