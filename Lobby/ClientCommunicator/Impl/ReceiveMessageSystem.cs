using System;
using System.Collections.Generic;
using Lobby.ClientCommunicator.API;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Common.ClientECSCommon.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;
using Event = Platform.Kernel.ECS.ClientEntitySystem.API.Event;
using Object = UnityEngine.Object;

namespace Lobby.ClientCommunicator.Impl {
    public class ReceiveMessageSystem : ECSSystem {
        readonly List<MessageNodeForSort> clonedMessages = new();

        [OnEventFire]
        public void ShowReceivedMessage(NodeAddedEvent e, ChatGUINode chatGUI, [JoinByScreen] [Context] ChatNode chat,
            [Context] [JoinByChat] ICollection<MessageNode> messageNodes, [JoinAll] SelfUserNode selfUser) {
            RectTransform component = chatGUI.chatContentGUI.GetComponent<RectTransform>();

            foreach (MessageNode messageNode in messageNodes) {
                RectTransform rectTransform = CreateMessageView(chatGUI, messageNode, selfUser);
                rectTransform.SetParent(component, false);
            }

            ScrollToBottom(component);
            LayoutRebuilder.ForceRebuildLayoutImmediate(chatGUI.chatContentGUI.GetComponent<RectTransform>());
            ScheduleEvent<SortChatMessagesEvent>(chatGUI);
        }

        RectTransform CreateMessageView(ChatGUINode chatGUI, MessageNode messageNode, SelfUserNode selfUser) {
            DateTime dateTime = new(messageNode.createdTimestamp.Timestamp * 10000);
            RectTransform rectTransform = (RectTransform)Object.Instantiate(chatGUI.chatContentGUI.MessagePrefab).transform;
            rectTransform.GetComponent<EntityBehaviour>().BuildEntity(messageNode.Entity);
            ChatMessageGUIComponent component = rectTransform.GetComponent<ChatMessageGUIComponent>();
            component.Text = messageNode.chatMessage.Message;
            Entity author = messageNode.chatMessageAuthor.Author;
            component.Username = author.GetComponent<UserUidComponent>().Uid;
            component.Time = string.Format("{0:H:mm}", dateTime);
            component.Self = author.Equals(selfUser.Entity);
            return rectTransform;
        }

        void ScrollToBottom(RectTransform scrollRectContent) => scrollRectContent.offsetMin = Vector2.zero;

        [OnEventFire]
        public void SortMessagesByTimestamp(SortChatMessagesEvent e, ChatGUINode chatGUI, [JoinByScreen] ChatNode chat,
            [JoinByChat] ICollection<MessageNodeForSort> messages) {
            clonedMessages.Clear();
            clonedMessages.AddRange(messages);
            clonedMessages.Sort();

            for (int i = 0; i < clonedMessages.Count; i++) {
                clonedMessages[i].chatMessageGUI.transform.SetSiblingIndex(i);
            }

            chatGUI.lazyScrollableVerticalList.AdjustChildrenVisibility();
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserComponent user;
        }

        public class ChatGUINode : Node {
            public ChatContentGUIComponent chatContentGUI;

            public LazyScrollableVerticalListComponent lazyScrollableVerticalList;

            public ScreenGroupComponent screenGroup;
        }

        public class ChatNode : Node {
            public ChatComponent chat;
            public ChatGroupComponent chatGroup;

            public ScreenGroupComponent screenGroup;
        }

        public class MessageNode : Node {
            public ChatGroupComponent chatGroup;

            public ChatMessageComponent chatMessage;

            public ChatMessageAuthorComponent chatMessageAuthor;

            public CreatedTimestampComponent createdTimestamp;
        }

        public class MessageNodeForSort : Node, IComparable<MessageNodeForSort> {
            public ChatMessageComponent chatMessage;

            public ChatMessageGUIComponent chatMessageGUI;

            public CreatedTimestampComponent createdTimestamp;

            public int CompareTo(MessageNodeForSort other) =>
                (int)(createdTimestamp.Timestamp - other.createdTimestamp.Timestamp);
        }

        public class SortChatMessagesEvent : Event { }
    }
}