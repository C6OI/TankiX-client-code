using Lobby.ClientCommunicator.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatChannelSwitchSystem : ECSSystem {
        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void SetGeneralChannelLoaded(NodeAddedEvent e,
            NotLoadedGeneralChatChannelNode notLoadedGeneralChatChannelNode) {
            notLoadedGeneralChatChannelNode.Entity.AddComponent<ActiveChannelComponent>();
            notLoadedGeneralChatChannelNode.Entity.AddComponent<LoadedChannelComponent>();
        }

        [OnEventFire]
        public void SetTeamChannelLoaded(NodeAddedEvent e, BattleChatGUINode battleChatGUINode,
            ActiveGeneralChatChannelNode activeGeneralChatChannelNode,
            NotLoadedTeamChatChannelNode notLoadedTeamChatChannelNode, [JoinByTeam] TeamNode teamNode) {
            activeGeneralChatChannelNode.Entity.RemoveComponent<ActiveChannelComponent>();
            notLoadedTeamChatChannelNode.Entity.AddComponent<ActiveChannelComponent>();
            SetActiveChannelGUI(battleChatGUINode, teamNode.teamColor.TeamColor);
            notLoadedTeamChatChannelNode.Entity.AddComponent<LoadedChannelComponent>();
        }

        [OnEventFire]
        public void SwitchToGeneralChannelOnTeamChatRemove(NodeRemoveEvent e, TeamChatChannelNode teamChatChannelNode,
            [JoinByBattle] InactiveGeneralChatChannelNode inactiveChannelNode,
            [JoinByScreen] BattleChatGUINode battleChatGUINode) {
            if (teamChatChannelNode.Entity.HasComponent<ActiveChannelComponent>()) {
                inactiveChannelNode.Entity.AddComponent<ActiveChannelComponent>();
                SetActiveChannelGUI(battleChatGUINode, TeamColor.NONE);
            }
        }

        [OnEventFire]
        public void SwitchChannelOnTabPressed(UpdateEvent e, ActiveChannelNode activeChannelNode,
            [JoinByScreen] InactiveChannelNode inactiveChannelNode) {
            if (InputManager.GetActionKeyDown(BattleChatActions.SWITCH_CHANNEL)) {
                ScheduleEvent<BattleChannelSwitchEvent>(inactiveChannelNode);
            }
        }

        [OnEventFire]
        public void SwitchToTeamChannel(BattleChannelSwitchEvent e, InactiveTeamChatChannelNode inactiveChannelNode,
            [JoinByTeam] TeamNode teamNode, [JoinByBattle] ActiveChannelNode activeChannelNode,
            [JoinByScreen] BattleChatGUINode battleChatGUINode) {
            SwitchActiveChannel(activeChannelNode, inactiveChannelNode);
            SetActiveChannelGUI(battleChatGUINode, teamNode.teamColor.TeamColor);
        }

        [OnEventFire]
        public void SwitchToGeneralChannel(BattleChannelSwitchEvent e, InactiveGeneralChatChannelNode inactiveChannelNode,
            [JoinByBattle] ActiveChannelNode activeChannelNode, [JoinByScreen] BattleChatGUINode battleChatGUINode) {
            SwitchActiveChannel(activeChannelNode, inactiveChannelNode);
            SetActiveChannelGUI(battleChatGUINode, TeamColor.NONE);
        }

        void SwitchActiveChannel(ActiveChannelNode activeChannelNode, InactiveChannelNode inactiveChannelNode) {
            activeChannelNode.Entity.RemoveComponent<ActiveChannelComponent>();
            inactiveChannelNode.Entity.AddComponent<ActiveChannelComponent>();
        }

        void SetActiveChannelGUI(BattleChatGUINode battleChatGUINode, TeamColor teamColor) {
            BattleChatGUIComponent battleChatGUI = battleChatGUINode.battleChatGUI;
            BattleChatLocalizedStringsComponent battleChatLocalizedStrings = battleChatGUINode.battleChatLocalizedStrings;
            string empty = string.Empty;
            Color color = default;

            switch (teamColor) {
                case TeamColor.BLUE:
                    battleChatGUI.InputFieldColor = battleChatGUI.BlueTeamNicknameColor;
                    color = battleChatGUI.BlueTeamNicknameColor;
                    empty = battleChatLocalizedStrings.TeamChatInputHint;
                    break;

                case TeamColor.RED:
                    battleChatGUI.InputFieldColor = battleChatGUI.RedTeamNicknameColor;
                    color = battleChatGUI.RedTeamNicknameColor;
                    empty = battleChatLocalizedStrings.TeamChatInputHint;
                    break;

                default:
                    battleChatGUI.InputFieldColor = battleChatGUI.CommonTextColor;
                    color = battleChatGUI.CommonTextColor;
                    empty = battleChatLocalizedStrings.GeneralChatInputHint;
                    break;
            }

            battleChatGUI.InputHintText = string.Format("{0}:", empty);
            battleChatGUI.InputHintColor = new Color(color.r, color.g, color.b, battleChatGUI.InputHintColor.a);
        }

        public class ChatChannelNode : Node {
            public ChatComponent chat;

            public ChatGroupComponent chatGroup;
        }

        public class TeamChatChannelNode : ChatChannelNode {
            public TeamBattleChatComponent teamBattleChat;

            public TeamGroupComponent teamGroup;
        }

        public class GeneralChatChannelNode : ChatChannelNode {
            public GeneralBattleChatComponent generalBattleChat;
        }

        [Not(typeof(LoadedChannelComponent))]
        public class NotLoadedTeamChatChannelNode : TeamChatChannelNode { }

        [Not(typeof(LoadedChannelComponent))]
        public class NotLoadedGeneralChatChannelNode : GeneralChatChannelNode { }

        public class ActiveChannelNode : Node {
            public ActiveChannelComponent activeChannel;

            public BattleGroupComponent battleGroup;
            public ChatComponent chat;

            public ChatGroupComponent chatGroup;

            public ScreenGroupComponent screenGroup;
        }

        public class ActiveGeneralChatChannelNode : GeneralChatChannelNode {
            public ActiveChannelComponent activeChannel;
        }

        [Not(typeof(ActiveChannelComponent))]
        public class InactiveChannelNode : Node {
            public BattleGroupComponent battleGroup;
            public ChatComponent chat;

            public ChatGroupComponent chatGroup;

            public ScreenGroupComponent screenGroup;
        }

        public class InactiveGeneralChatChannelNode : InactiveChannelNode {
            public GeneralBattleChatComponent generalBattleChat;
        }

        public class InactiveTeamChatChannelNode : InactiveChannelNode {
            public TeamBattleChatComponent teamBattleChat;
        }

        public class BattleChatGUINode : Node {
            public BattleChatGUIComponent battleChatGUI;

            public BattleChatLocalizedStringsComponent battleChatLocalizedStrings;

            public ScreenGroupComponent screenGroup;
        }

        public class TeamNode : Node {
            public TeamColorComponent teamColor;
            public TeamGroupComponent teamGroup;
        }
    }
}