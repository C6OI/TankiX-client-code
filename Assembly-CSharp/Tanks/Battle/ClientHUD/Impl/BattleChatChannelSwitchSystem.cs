using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using Tanks.Lobby.ClientCommunicator.API;
using Tanks.Lobby.ClientCommunicator.Impl;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatChannelSwitchSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, ActiveHomeChannel activeChannel, [JoinAll] Optional<GeneralChatChannelNode> battleChat, [JoinAll] Dialog dialog) {
            if (!battleChat.IsPresent()) {
                dialog.chatDialog.AddUIMessage(e.Message);
            }
        }

        [OnEventFire]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, SingleNode<ChatChannelUIComponent> chat, [JoinAll] Optional<GeneralChatChannelNode> battleChat,
            [JoinAll] Dialog dialog) {
            if (!battleChat.IsPresent()) {
                dialog.chatDialog.SetLastMessage(e.Message);
            }
        }

        [OnEventFire]
        public void OnEnterBattle(NodeAddedEvent e, GeneralChatChannelNode battleChat, [JoinAll] ActiveHomeChannel activeChannel, [JoinAll] Dialog dialog) {
            dialog.chatDialog.SelectChannel(activeChannel.chatChannel.ChatType, new List<ChatMessage>());
        }

        [OnEventFire]
        public void OnExitBattle(NodeRemoveEvent e, GeneralChatChannelNode battleChat, [JoinAll] ActiveHomeChannel activeChannel, [JoinAll] Dialog dialog) {
            dialog.chatDialog.SelectChannel(activeChannel.chatChannel.ChatType, activeChannel.chatChannel.Messages);
        }

        [OnEventFire]
        public void SetGeneralChannelLoaded(NodeAddedEvent e, NotLoadedGeneralChatChannelNode notLoadedGeneralChatChannelNode, BattleChatGUINode battleChatGUINode) {
            SetActiveChannelGUI(battleChatGUINode, TeamColor.NONE);
            notLoadedGeneralChatChannelNode.Entity.AddComponent<ActiveBattleChannelComponent>();
            notLoadedGeneralChatChannelNode.Entity.AddComponent<LoadedChannelComponent>();
        }

        [OnEventFire]
        public void SetTeamChannelLoaded(NodeAddedEvent e, BattleChatGUINode battleChatGUINode, ActiveGeneralChatChannelNode activeGeneralChatChannelNode,
            NotLoadedTeamChatChannelNode notLoadedTeamChatChannelNode, [JoinAll] SelfUserTeamNode selfUserTeamNode, [JoinByTeam] TeamNode teamNode) {
            activeGeneralChatChannelNode.Entity.RemoveComponent<ActiveBattleChannelComponent>();
            notLoadedTeamChatChannelNode.Entity.AddComponent<ActiveBattleChannelComponent>();
            SetActiveChannelGUI(battleChatGUINode, teamNode.colorInBattle.TeamColor);
            notLoadedTeamChatChannelNode.Entity.AddComponent<LoadedChannelComponent>();
        }

        [OnEventFire]
        public void SwitchToGeneralChannelOnTeamChatRemove(NodeRemoveEvent e, TeamChatChannelNode teamChatChannelNode, [JoinAll] InactiveGeneralChatChannelNode inactiveChannelNode,
            [JoinByScreen] BattleChatGUINode battleChatGUINode) {
            if (teamChatChannelNode.Entity.HasComponent<ActiveBattleChannelComponent>()) {
                inactiveChannelNode.Entity.AddComponent<ActiveBattleChannelComponent>();
                SetActiveChannelGUI(battleChatGUINode, TeamColor.NONE);
            }
        }

        [OnEventFire]
        public void SwitchChannelOnTabPressed(UpdateEvent e, ActiveChannelNode activeChannelNode, [JoinByScreen] InactiveChannelNode inactiveChannelNode) {
            if (InputManager.GetActionKeyDown(BattleChatActions.SWITCH_CHANNEL)) {
                ScheduleEvent<BattleChannelSwitchEvent>(inactiveChannelNode);
            }
        }

        [OnEventFire]
        public void SwitchToTeamChannel(BattleChannelSwitchEvent e, InactiveTeamChatChannelNode inactiveChannelNode, [JoinAll] SelfUserTeamNode selfIUserNode,
            [JoinByTeam] TeamNode teamNode, [JoinAll] ActiveChannelNode activeChannelNode, [JoinByScreen] BattleChatGUINode battleChatGUINode) {
            SwitchActiveChannel(activeChannelNode, inactiveChannelNode);
            SetActiveChannelGUI(battleChatGUINode, teamNode.colorInBattle.TeamColor);
        }

        [OnEventFire]
        public void SwitchToGeneralChannel(BattleChannelSwitchEvent e, InactiveGeneralChatChannelNode inactiveChannelNode, [JoinAll] ActiveChannelNode activeChannelNode,
            [JoinByScreen] BattleChatGUINode battleChatGUINode) {
            SwitchActiveChannel(activeChannelNode, inactiveChannelNode);
            SetActiveChannelGUI(battleChatGUINode, TeamColor.NONE);
        }

        void SwitchActiveChannel(ActiveChannelNode activeChannelNode, InactiveChannelNode inactiveChannelNode) {
            activeChannelNode.Entity.RemoveComponent<ActiveBattleChannelComponent>();
            inactiveChannelNode.Entity.AddComponent<ActiveBattleChannelComponent>();
        }

        void SetActiveChannelGUI(BattleChatGUINode battleChatGUINode, TeamColor teamColor) {
            ChatUIComponent chatUI = battleChatGUINode.chatUI;
            BattleChatLocalizedStringsComponent battleChatLocalizedStrings = battleChatGUINode.battleChatLocalizedStrings;
            string empty = string.Empty;
            Color color = default;
            Color color2 = default;

            switch (teamColor) {
                case TeamColor.BLUE:
                    color2 = chatUI.BlueTeamNicknameColor;
                    color = chatUI.BlueTeamNicknameColor;
                    empty = battleChatLocalizedStrings.TeamChatInputHint;
                    break;

                case TeamColor.RED:
                    color2 = chatUI.RedTeamNicknameColor;
                    color = chatUI.RedTeamNicknameColor;
                    empty = battleChatLocalizedStrings.TeamChatInputHint;
                    break;

                default:
                    color2 = chatUI.CommonTextColor;
                    color = chatUI.CommonTextColor;
                    empty = battleChatLocalizedStrings.GeneralChatInputHint;
                    break;
            }

            chatUI.InputHintText = string.Format("{0}: ", empty);
            chatUI.InputHintColor = new Color(color.r, color.g, color.b, chatUI.InputHintColor.a);
            chatUI.InputTextColor = chatUI.InputHintColor;
            chatUI.BottomLineColor = color2;
            chatUI.SetHintSize(teamColor == TeamColor.BLUE || teamColor == TeamColor.RED);
        }

        public class SelfUserTeamNode : Node {
            public SelfBattleUserComponent selfBattleUser;
            public TeamGroupComponent teamGroup;
        }

        public class ChatChannelNode : Node {
            public ChatComponent chat;
        }

        public class TeamChatChannelNode : ChatChannelNode {
            public TeamBattleChatComponent teamBattleChat;
        }

        public class GeneralChatChannelNode : ChatChannelNode {
            public GeneralBattleChatComponent generalBattleChat;
        }

        [Not(typeof(LoadedChannelComponent))]
        public class NotLoadedTeamChatChannelNode : TeamChatChannelNode { }

        [Not(typeof(LoadedChannelComponent))]
        public class NotLoadedGeneralChatChannelNode : GeneralChatChannelNode { }

        public class ActiveChannelNode : Node {
            public ActiveBattleChannelComponent activeBattleChannel;
            public ChatComponent chat;

            public ScreenGroupComponent screenGroup;
        }

        public class ActiveGeneralChatChannelNode : GeneralChatChannelNode {
            public ActiveBattleChannelComponent activeBattleChannel;
        }

        [Not(typeof(ActiveBattleChannelComponent))]
        public class InactiveChannelNode : Node {
            public ChatComponent chat;

            public ScreenGroupComponent screenGroup;
        }

        public class ActiveHomeChannel : Node {
            public ActiveChannelComponent activeChannel;
            public ChatChannelComponent chatChannel;

            public ChatChannelUIComponent chatChannelUI;
        }

        public class Dialog : Node {
            public ChatDialogComponent chatDialog;
        }

        public class InactiveGeneralChatChannelNode : InactiveChannelNode {
            public GeneralBattleChatComponent generalBattleChat;
        }

        public class InactiveTeamChatChannelNode : InactiveChannelNode {
            public TeamBattleChatComponent teamBattleChat;
        }

        public class BattleChatGUINode : Node {
            public BattleChatLocalizedStringsComponent battleChatLocalizedStrings;
            public ChatUIComponent chatUI;

            public ScreenGroupComponent screenGroup;
        }

        public class TeamNode : Node {
            public ColorInBattleComponent colorInBattle;
            public TeamGroupComponent teamGroup;
        }
    }
}