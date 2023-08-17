using Lobby.ClientCommunicator.Impl;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class BattleChatVisibilitySystem : ECSSystem {
        static readonly string BATTLE_CHAT_SHOW_PREREQUISITE = "BATTLE_CHAT_SHOW_PREREQUISITE";

        static readonly string BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE = "BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE";

        [Inject] public static InputManager InputManager { get; set; }

        [OnEventFire]
        public void ShowBattleChatElements(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState,
            [Combine] BattleChatElementNode battleChatElementNode) =>
            battleChatElementNode.visibilityPrerequisites.AddShowPrerequisite(BATTLE_CHAT_SHOW_PREREQUISITE);

        [OnEventFire]
        public void HideBattleChatElements(NodeAddedEvent e, SingleNode<BattleActionsStateComponent> battleActionsState,
            [Combine] BattleChatElementNode battleChatElementNode) =>
            battleChatElementNode.visibilityPrerequisites.RemoveShowPrerequisite(BATTLE_CHAT_SHOW_PREREQUISITE);

        [OnEventFire]
        public void ShowBattleChatMessagesOnChatState(NodeAddedEvent e, BattleChatStateNode battleChatStateNode,
            [JoinByScreen] ChatContentNode chatContentNode) =>
            chatContentNode.visibilityPrerequisites.AddShowPrerequisite(BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE);

        [OnEventFire]
        public void DisableHideMessagesOnChatState(NodeAddedEvent e, BattleChatStateNode battleChatStateNode,
            [JoinByScreen] ChatContentWithSheduleNode chatContentWithSheduleNode) =>
            DisableHideMessagesSchedule(chatContentWithSheduleNode);

        [OnEventFire]
        public void HideMessagesOnShowScore(UpdateEvent e, ChatContentWithSheduleNode chatContentWithSheduleNode,
            [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState) {
            if (InputManager.CheckAction(BattleActions.SHOW_SCORE)) {
                DisableHideMessagesSchedule(chatContentWithSheduleNode);

                chatContentWithSheduleNode.visibilityPrerequisites.RemoveShowPrerequisite(
                    BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE);
            }
        }

        [OnEventFire]
        public void HideBattleChatMessages(NodeRemoveEvent e, BattleChatStateNode battleChatStateNode,
            [JoinByScreen] ChatContentNode chatContentNode) => HideMessagesDelayed(chatContentNode);

        [OnEventFire]
        public void DisableHideMessagesOnMessageReceived(NodeAddedEvent e,
            SingleNode<BattleChatMessageGUIComponent> battleChatMessageGUI, ChatContentNode chatContentNode,
            [JoinByScreen] ChatContentWithSheduleNode chatContentWithSheduleNode,
            [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState) =>
            DisableHideMessagesSchedule(chatContentWithSheduleNode);

        [OnEventComplete]
        public void ShowBattleChatMessagesOnMessageReceived(NodeAddedEvent e,
            SingleNode<BattleChatMessageGUIComponent> battleChatMessageGUI, ChatContentNode chatContentNode,
            [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState) {
            chatContentNode.visibilityPrerequisites.AddShowPrerequisite(BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE);
            HideMessagesDelayed(chatContentNode);
        }

        void HideMessagesDelayed(ChatContentNode chatContentNode) {
            ScheduledEvent scheduledEvent = NewEvent<StopVisiblePeriodEvent>().Attach(chatContentNode)
                .ScheduleDelayed(chatContentNode.visibilityInterval.intervalInSec);

            chatContentNode.Entity.AddComponent(new HideBattleChatMessagesSheduleComponent(scheduledEvent));
        }

        void DisableHideMessagesSchedule(ChatContentWithSheduleNode chatContentWithSheduleNode) {
            chatContentWithSheduleNode.hideBattleChatMessagesShedule.ScheduledEvent.Manager().Cancel();
            chatContentWithSheduleNode.Entity.RemoveComponent<HideBattleChatMessagesSheduleComponent>();
        }

        [OnEventFire]
        public void HideBattleChatMessages(StopVisiblePeriodEvent e, ChatContentWithSheduleNode chatContentWithSheduleNode,
            [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState) {
            chatContentWithSheduleNode.visibilityPrerequisites.RemoveShowPrerequisite(
                BATTLE_CHAT_SHOW_MESSAGES_PREREQUISITE);

            chatContentWithSheduleNode.Entity.RemoveComponent<HideBattleChatMessagesSheduleComponent>();
        }

        public class BattleChatElementNode : Node {
            public ShowWhileBattleChatIsActiveComponent showWhileBattleChatIsActive;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class ChatContentNode : Node {
            public ChatContentGUIComponent chatContentGui;

            public ScreenGroupComponent screenGroup;

            public VisibilityIntervalComponent visibilityInterval;

            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class ChatContentWithSheduleNode : ChatContentNode {
            public HideBattleChatMessagesSheduleComponent hideBattleChatMessagesShedule;
        }

        public class BattleChatStateNode : Node {
            public BattleChatStateComponent battleChatState;

            public ScreenGroupComponent screenGroup;
        }
    }
}