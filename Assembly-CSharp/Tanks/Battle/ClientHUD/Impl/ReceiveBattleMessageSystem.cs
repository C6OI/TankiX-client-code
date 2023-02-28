using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientBattleSelect.API;
using Tanks.Lobby.ClientBattleSelect.Impl;
using Tanks.Lobby.ClientCommunicator.API;
using Tanks.Lobby.ClientCommunicator.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ReceiveBattleMessageSystem : ECSSystem {
        [OnEventFire]
        public void ShowReceivedMessage(ChatMessageReceivedEvent e, ChatNode chatNode, [JoinAll] SelfUserNode selfUser) {
            if (e.SystemMessage) {
                NewEvent(new BattleChatSystemMessageReceivedEvent(e.Message)).Attach(chatNode).Schedule();
            } else if (!selfUser.blackList.BlockedUsers.Contains(e.UserId)) {
                NewEvent(new BattleChatValidMessageReceivedEvent(e.Message, e.UserId)).Attach(chatNode).Schedule();
            }
        }

        [OnEventFire]
        public void ShowReceivedTeamMessage(BattleChatSystemMessageReceivedEvent e, ChatNode chatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode,
            [JoinByScreen] ChatContentGUINode chatContentGUINode) {
            ChatUIComponent chatUI = battleChatGUINode.chatUI;
            GameObject gameObject = Object.Instantiate(chatContentGUINode.chatContentGUI.MessagePrefab);
            Entity entity = gameObject.GetComponent<EntityBehaviour>().Entity;
            Color systemMessageColor = chatUI.SystemMessageColor;
            ChatMessageUIComponent component = gameObject.GetComponent<ChatMessageUIComponent>();
            component.FirstPartText = string.Empty;
            component.SecondPartText = e.Message;
            component.SecondPartTextColor = systemMessageColor;
            RectTransform component2 = chatContentGUINode.chatContentGUI.gameObject.GetComponent<RectTransform>();
            gameObject.transform.SetParent(component2, false);
            component2.offsetMin = Vector2.zero;
            ScheduleEvent<ResizeBattleChatScrollViewEvent>(entity);
            chatUI.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
        }

        [OnEventFire]
        public void ShowReceivedMessage(BattleChatValidMessageReceivedEvent e, BattleChatNode chatNode, [JoinAll] SelfBattleUserNode selfBattleUser,
            [Combine] [JoinByBattle] BattleUserNode battleUserNode, [JoinByUser] UserNode userNode) {
            if (e.UserId == userNode.Entity.Id) {
                NewEvent(new BattleChatUserMessageReceivedEvent(e.Message)).Attach(chatNode).Attach(userNode).Schedule();
            }
        }

        [OnEventFire]
        public void ShowReceivedMessage(BattleChatValidMessageReceivedEvent e, BattleLobbyChatNode chatNode, [JoinAll] SelfBattleLobbyUserNode selfBattleLobbyUser,
            [Combine] [JoinByBattleLobby] BattleLobbyUserNode battleLobbyUserNode) {
            if (e.UserId == battleLobbyUserNode.Entity.Id) {
                NewEvent(new BattleChatUserMessageReceivedEvent(e.Message)).Attach(chatNode).Attach(battleLobbyUserNode).Schedule();
            }
        }

        [OnEventFire]
        public void ShowReceivedTeamMessage(BattleChatUserMessageReceivedEvent e, UserNode userNode, TeamChatNode teamChatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode,
            [JoinByScreen] ChatContentGUINode chatContentGUINode, [JoinAll] SelfUserTeamNode selfUserTeamNode, [JoinByTeam] TeamNode teamNode) {
            CreateMessage(chatContentGUINode, battleChatGUINode, userNode.Entity, e.Message, true, teamNode.colorInBattle.TeamColor);
        }

        [OnEventFire]
        public void ShowReceivedGeneralMessage(BattleChatUserMessageReceivedEvent e, UserNode userNode, [JoinByUser] NotTeamBattleUserNode notTeamBattleUserNode,
            GeneralChatNode generalChatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode) {
            CreateMessage(chatContentGUINode, battleChatGUINode, userNode.Entity, e.Message, false, notTeamBattleUserNode.colorInBattle.TeamColor);
        }

        [OnEventFire]
        public void ShowReceivedGeneralMessage(BattleChatUserMessageReceivedEvent e, UserNode userNode, [JoinByUser] TeamBattleUserNode teamBattleUserNode, [JoinByTeam] TeamNode teamNode,
            GeneralChatNode generalChatNode, [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode) {
            CreateMessage(chatContentGUINode, battleChatGUINode, userNode.Entity, e.Message, false, teamNode.colorInBattle.TeamColor);
        }

        [OnEventFire]
        public void ShowReceivedGeneralMessage(BattleChatUserMessageReceivedEvent e, UserNode user, SingleNode<BattleLobbyChatComponent> lobbyChat,
            [JoinByScreen] BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode, [JoinAll] SingleNode<SelfUserComponent> selfUser) {
            TeamColor teamColor = user.Entity.HasComponent<TeamColorComponent>() ? user.Entity.GetComponent<TeamColorComponent>().TeamColor : TeamColor.NONE;
            TeamColor teamColor2 = selfUser.Entity.HasComponent<TeamColorComponent>() ? selfUser.Entity.GetComponent<TeamColorComponent>().TeamColor : TeamColor.NONE;
            TeamColor teamColor3 = (teamColor != teamColor2 || teamColor == TeamColor.NONE) && !user.Entity.Equals(selfUser.Entity) ? TeamColor.RED : TeamColor.BLUE;
            CreateMessage(chatContentGUINode, battleChatGUINode, user.Entity, e.Message, false, teamColor3);
        }

        void CreateMessage(ChatContentGUINode chatContentGUINode, BattleChatGUINode battleChatGUINode, Entity user, string message, bool isTeamMessage, TeamColor teamColor) {
            ChatUIComponent chatUI = battleChatGUINode.chatUI;
            GameObject gameObject = Object.Instantiate(chatContentGUINode.chatContentGUI.MessagePrefab);
            Entity entity = gameObject.GetComponent<EntityBehaviour>().Entity;
            Color firstPartTextColor;
            Color secondPartTextColor;

            switch (teamColor) {
                case TeamColor.BLUE:
                    firstPartTextColor = chatUI.BlueTeamNicknameColor;
                    secondPartTextColor = !isTeamMessage ? chatUI.CommonTextColor : chatUI.BlueTeamTextColor;
                    break;

                case TeamColor.RED:
                    firstPartTextColor = chatUI.RedTeamNicknameColor;
                    secondPartTextColor = !isTeamMessage ? chatUI.CommonTextColor : chatUI.RedTeamTextColor;
                    break;

                default:
                    firstPartTextColor = chatUI.CommonNicknameColor;
                    secondPartTextColor = chatUI.CommonTextColor;
                    break;
            }

            ChatMessageUIComponent component = gameObject.GetComponent<ChatMessageUIComponent>();
            component.FirstPartText = user.GetComponent<UserUidComponent>().Uid + ": ";
            component.FirstPartTextColor = firstPartTextColor;
            component.SecondPartText = message;
            component.SecondPartTextColor = secondPartTextColor;
            entity.AddComponent(new UserGroupComponent(user));
            RectTransform component2 = chatContentGUINode.chatContentGUI.gameObject.GetComponent<RectTransform>();
            gameObject.transform.SetParent(component2, false);
            ScheduleEvent<ResizeBattleChatScrollViewEvent>(entity);
            chatUI.SendMessage("RefreshCurve", SendMessageOptions.DontRequireReceiver);
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserUidComponent userUid;
        }

        public class BattleUserNode : Node {
            public BattleUserComponent battleUser;

            public UserGroupComponent userGroup;
        }

        public class BattleLobbyUserNode : Node {
            public BattleLobbyGroupComponent battleLobbyGroup;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class SelfBattleLobbyUserNode : BattleLobbyUserNode {
            public SelfUserComponent selfUser;
        }

        public class TeamBattleUserNode : BattleUserNode {
            public TeamGroupComponent teamGroup;
        }

        [Not(typeof(TeamGroupComponent))]
        public class NotTeamBattleUserNode : BattleUserNode {
            public ColorInBattleComponent colorInBattle;
        }

        public class TeamNode : Node {
            public ColorInBattleComponent colorInBattle;
            public TeamGroupComponent teamGroup;
        }

        public class SelfUserNode : Node {
            public BlackListComponent blackList;
            public SelfUserComponent selfUser;
        }

        public class SelfBattleUserNode : Node {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class SelfUserTeamNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public TeamGroupComponent teamGroup;
        }

        public class ChatNode : Node {
            public ChatComponent chat;

            public ScreenGroupComponent screenGroup;
        }

        public class GeneralChatNode : ChatNode {
            public GeneralBattleChatComponent generalBattleChat;
        }

        public class TeamChatNode : ChatNode {
            public TeamBattleChatComponent teamBattleChat;
        }

        [Not(typeof(BattleLobbyChatComponent))]
        public class BattleChatNode : ChatNode { }

        public class BattleLobbyChatNode : ChatNode {
            public BattleLobbyChatComponent battleLobbyChat;
        }

        public class ChatContentGUINode : Node {
            public ChatContentGUIComponent chatContentGUI;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleChatGUINode : Node {
            public ChatUIComponent chatUI;

            public LazyScrollableVerticalListComponent lazyScrollableVerticalList;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleChatValidMessageReceivedEvent : Event {
            public BattleChatValidMessageReceivedEvent(string message, long userId) {
                Message = message;
                UserId = userId;
            }

            public string Message { get; set; }

            public long UserId { get; set; }
        }

        public class BattleChatSystemMessageReceivedEvent : Event {
            public BattleChatSystemMessageReceivedEvent(string message) => Message = message;

            public string Message { get; set; }
        }

        public class BattleChatUserMessageReceivedEvent : Event {
            public BattleChatUserMessageReceivedEvent(string message) => Message = message;

            public string Message { get; set; }
        }
    }
}