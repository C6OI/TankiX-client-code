using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientCommunicator.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ResizeBattleChatScrollViewSystem : ECSSystem {
        [OnEventFire]
        public void ResizeChatOnChatState(NodeAddedEvent e, SingleNode<BattleChatStateComponent> battleChatState, [Combine] BattleChatGUINode battleChatGUINode) {
            ResizeScrollView(battleChatGUINode, true);
        }

        [OnEventFire]
        public void ResizeChatOnActionsState(NodeAddedEvent e, SingleNode<BattleActionsStateComponent> battleActionsState, [Combine] BattleChatGUINode battleChatGUINode,
            [Combine] [JoinByScreen] ChatContentGUINode chatContentGUINode) {
            chatContentGUINode.chatContentGUI.gameObject.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            ResizeScrollView(battleChatGUINode, false);
        }

        [OnEventFire]
        public void ResizeSpectatorChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode, [JoinAll] BattleChatSpectatorGUINode battleChatSpectatorGUINode) {
            ResizeScrollView(battleChatSpectatorGUINode, false);
        }

        [OnEventFire]
        public void ResizeChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode, [JoinAll] SingleNode<BattleChatStateComponent> battleChatState,
            [JoinAll] [Combine] BattleChatGUINode battleChatGUINode) {
            ResizeScrollView(battleChatGUINode, true);
        }

        [OnEventFire]
        public void ResizeChatOnMessageResized(ResizeBattleChatScrollViewEvent e, Node anyNode, [JoinAll] SingleNode<BattleActionsStateComponent> battleActionsState,
            [JoinAll] [Combine] BattleChatGUINode battleChatGUINode) {
            ResizeScrollView(battleChatGUINode, false);
        }

        void ResizeScrollView(BattleChatGUINode battleChatGUINode, bool chatIsActive) {
            battleChatGUINode.lazyScrollableVerticalList.AdjustPlaceholdersSiblingIndices();
            ChatUIComponent chatUI = battleChatGUINode.chatUI;
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatUI.MessagesContainer.gameObject.GetComponent<RectTransform>());
            int num = chatUI.MessagesContainer.transform.childCount - 2;

            if (num != 0) {
                int num2 = !chatIsActive ? chatUI.MaxVisibleMessagesInPassiveState : chatUI.MaxVisibleMessagesInActiveState;
                int num3 = Mathf.Min(num, num2);
                int num4 = num;
                float num5 = 0f;

                while (num3 > 0) {
                    num5 += chatUI.MessagesContainer.transform.GetChild(num4).GetComponent<RectTransform>().sizeDelta.y;
                    num4--;
                    num3--;
                }

                chatUI.ScrollViewHeight = num5;
                ChangeScrollBarActivity(chatUI, chatIsActive, num, num2);
            }
        }

        void ChangeScrollBarActivity(ChatUIComponent chatUI, bool chatIsActive, int messagesCount, int maxMessagesCount) {
            chatUI.ScrollBarActivity = chatIsActive && (messagesCount > maxMessagesCount || chatUI.ScrollViewPosY >= 0f);
        }

        public class BattleChatGUINode : Node {
            public ChatUIComponent chatUI;

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