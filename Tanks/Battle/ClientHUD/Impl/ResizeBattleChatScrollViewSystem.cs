using Lobby.ClientCommunicator.Impl;
using Lobby.ClientControls.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ResizeBattleChatScrollViewSystem : ECSSystem {
        [OnEventFire]
        public void ResizeChatOnChatState(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState,
            BattleChatGUINode battleChatGUINode) => ResizeScrollView(battleChatGUINode, true);

        [OnEventFire]
        public void ResizeChatOnActionsState(NodeAddedEvent e, SingleNode<BattleActionsStateComponent> battleActionsState,
            BattleChatGUINode battleChatGUINode, [JoinByScreen] ChatContentGUINode chatContentGUINode) {
            chatContentGUINode.chatContentGUI.gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            ResizeScrollView(battleChatGUINode, false);
        }

        [OnEventFire]
        public void ResizeSpectatorChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode,
            [JoinAll] BattleChatSpectatorGUINode battleChatSpectatorGUINode) =>
            ResizeScrollView(battleChatSpectatorGUINode, false);

        [OnEventFire]
        public void ResizeChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode,
            [JoinAll] SingleNode<BattleChatStateComponent> battleChatState, [JoinAll] BattleChatGUINode battleChatGUINode) =>
            ResizeScrollView(battleChatGUINode, true);

        [OnEventFire]
        public void ResizeChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode,
            [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState,
            [JoinAll] BattleChatGUINode battleChatGUINode) => ResizeScrollView(battleChatGUINode, false);

        void ResizeScrollView(BattleChatGUINode battleChatGUINode, bool chatIsActive) {
            battleChatGUINode.lazyScrollableVerticalList.AdjustPlaceholdersSiblingIndices();
            BattleChatGUIComponent battleChatGUI = battleChatGUINode.battleChatGUI;

            LayoutRebuilder.ForceRebuildLayoutImmediate(battleChatGUI.MessagesContainer.gameObject
                .GetComponent<RectTransform>());

            int num = battleChatGUI.MessagesContainer.transform.childCount - 2;

            if (num != 0) {
                int num2 = !chatIsActive ? battleChatGUI.MaxVisibleMessagesInPassiveState
                               : battleChatGUI.MaxVisibleMessagesInActiveState;

                int num3 = Mathf.Min(num, num2);
                int num4 = num;
                float num5 = 0f;

                while (num3 > 0) {
                    num5 += battleChatGUI.MessagesContainer.transform.GetChild(num4).GetComponent<RectTransform>().sizeDelta
                        .y;

                    num4--;
                    num3--;
                }

                battleChatGUI.ScrollViewHeight = num5;
                ChangeScrollBarActivity(battleChatGUI, chatIsActive, num, num2);
            }
        }

        void ChangeScrollBarActivity(BattleChatGUIComponent battleChatGUI, bool chatIsActive, int messagesCount,
            int maxMessagesCount) => battleChatGUI.ScrollBarActivity =
                                         chatIsActive &&
                                         (messagesCount > maxMessagesCount || battleChatGUI.ScrollViewPosY >= 0f);

        public class BattleChatGUINode : Node {
            public BattleChatGUIComponent battleChatGUI;

            public LazyScrollableVerticalListComponent lazyScrollableVerticalList;

            public ScreenGroupComponent screenGroup;
        }

        public class BattleChatSpectatorGUINode : BattleChatGUINode {
            public BattleChatSpectatorComponent battleChatSpectator;
        }

        public class ChatContentGUINode : Node {
            public ChatContentGUIComponent chatContentGUI;

            public ScreenGroupComponent screenGroup;
        }
    }
}