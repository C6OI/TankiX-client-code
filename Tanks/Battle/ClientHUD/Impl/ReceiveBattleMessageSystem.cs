using Lobby.ClientCommunicator.API;
using Lobby.ClientCommunicator.Impl;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ReceiveBattleMessageSystem : ECSSystem {
        [OnEventFire]
        public void ShowReceivedTeamMessage(BattleChatMessageReceivedEvent e, UserNode userNode, TeamChatNode teamChatNode,
            [JoinByTeam] TeamNode teamNode, [JoinAll] BattleChatGUINode battleChatGUINode,
            [JoinByScreen] ChatContentGUINode chatContentGUINode) => CreateMessage(chatContentGUINode,
            battleChatGUINode,
            userNode.Entity,
            e.Message,
            true,
            teamNode.teamColor.TeamColor);

        [OnEventFire]
        public void ShowReceivedGeneralMessage(BattleChatMessageReceivedEvent e, UserNode userNode,
            [JoinByUser] NotTeamBattleUserNode notTeamBattleUserNode, GeneralChatNode generalChatNode,
            [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode) =>
            CreateMessage(chatContentGUINode, battleChatGUINode, userNode.Entity, e.Message, false, TeamColor.NONE);

        [OnEventFire]
        public void ShowReceivedGeneralMessage(BattleChatMessageReceivedEvent e, UserNode userNode,
            [JoinByUser] TeamBattleUserNode teamBattleUserNode, [JoinByTeam] TeamNode teamNode,
            GeneralChatNode generalChatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode,
            [JoinByScreen] ChatContentGUINode chatContentGUINode) => CreateMessage(chatContentGUINode,
            battleChatGUINode,
            userNode.Entity,
            e.Message,
            false,
            teamNode.teamColor.TeamColor);

        void CreateMessage(ChatContentGUINode chatContentGUINode, BattleChatGUINode battleChatGUINode, Entity user,
            string message, bool isTeamMessage, TeamColor teamColor) {
            BattleChatGUIComponent battleChatGUI = battleChatGUINode.battleChatGUI;
            GameObject gameObject = CreateMessageObject(chatContentGUINode, user);
            BattleChatMessageGUIComponent component = gameObject.GetComponent<BattleChatMessageGUIComponent>();
            Color color;
            Color color2;

            switch (teamColor) {
                case TeamColor.BLUE:
                    color = battleChatGUI.BlueTeamNicknameColor;
                    color2 = !isTeamMessage ? battleChatGUI.CommonTextColor : battleChatGUI.BlueTeamTextColor;
                    break;

                case TeamColor.RED:
                    color = battleChatGUI.RedTeamNicknameColor;
                    color2 = !isTeamMessage ? battleChatGUI.CommonTextColor : battleChatGUI.RedTeamTextColor;
                    break;

                default:
                    color = battleChatGUI.CommonNicknameColor;
                    color2 = battleChatGUI.CommonTextColor;
                    break;
            }

            component.Text = string.Format("<color=#{0}>{1}</color><color=#{2}>: {3}</color>",
                ColorUtility.ToHtmlStringRGBA(color),
                user.GetComponent<UserUidComponent>().Uid,
                ColorUtility.ToHtmlStringRGBA(color2),
                message);

            RectTransform component2 = chatContentGUINode.chatContentGUI.gameObject.GetComponent<RectTransform>();
            gameObject.transform.SetParent(component2, false);
            component2.offsetMin = Vector2.zero;
            ScheduleEvent<ResizeBattleChatScrollViewEvent>(gameObject.GetComponent<EntityBehaviour>().Entity);
        }

        GameObject CreateMessageObject(ChatContentGUINode chatContentGUINode, Entity user) {
            GameObject gameObject = Object.Instantiate(chatContentGUINode.chatContentGUI.MessagePrefab);
            gameObject.GetComponent<EntityBehaviour>().Entity.AddComponent(new UserGroupComponent(user));
            return gameObject;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserUidComponent userUid;
        }

        public class BattleUserNode : Node {
            public BattleUserComponent battleUser;

            public UserGroupComponent userGroup;
        }

        public class TeamBattleUserNode : BattleUserNode {
            public TeamGroupComponent teamGroup;
        }

        [Not(typeof(TeamGroupComponent))]
        public class NotTeamBattleUserNode : BattleUserNode { }

        public class TeamNode : Node {
            public TeamColorComponent teamColor;
            public TeamGroupComponent teamGroup;
        }

        public class ChatNode : Node {
            public ChatComponent chat;
            public ChatGroupComponent chatGroup;

            public ScreenGroupComponent screenGroup;
        }

        public class GeneralChatNode : ChatNode {
            public GeneralBattleChatComponent generalBattleChat;
        }

        public class TeamChatNode : ChatNode {
            public TeamBattleChatComponent teamBattleChat;

            public TeamGroupComponent teamGroup;
        }

        public class ChatContentGUINode : Node {
            public ChatContentGUIComponent chatContentGUI;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleChatGUINode : Node {
            public BattleChatGUIComponent battleChatGUI;

            public LazyScrollableVerticalListComponent lazyScrollableVerticalList;

            public ScreenGroupComponent screenGroup;
        }
    }
}