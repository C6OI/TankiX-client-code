using System.Collections.Generic;
using Lobby.ClientCommunicator.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Lobby.ClientUserProfile.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientCommunicator.Impl {
    public class ActiveUsersSystem : ECSSystem {
        [OnEventFire]
        public void ShowActiveUserOnEvent(UserEnterToChatEvent e, UserNotSelfNode userNode,
            SingleNode<ChatComponent> chatNode, [JoinByScreen] ChatActiveUsersNode chatActiveUsers) =>
            NewEvent<ShowActiveUserEvent>().Attach(chatActiveUsers).Attach(userNode).Schedule();

        [OnEventFire]
        public void ShowActiveUsersOnEnter(NodeAddedEvent e, ChatNode chat,
            [Context] [JoinByScreen] ChatActiveUsersNode chatActiveUsers) {
            List<Entity> users = chat.chatActiveUserList.Users;

            foreach (Entity item in users) {
                NewEvent<ShowActiveUserEvent>().Attach(chatActiveUsers).Attach(item).Schedule();
            }
        }

        [OnEventFire]
        [Mandatory]
        public void ShowActiveUser(ShowActiveUserEvent e, UserNode userNode, ChatActiveUsersNode chatActiveUsers) {
            RectTransform component = chatActiveUsers.chatActiveUsersGui.gameObject.GetComponent<RectTransform>();
            GameObject gameObject = new UserLabelBuilder(userNode.Entity.Id).SubscribeAvatarClick().Build();
            Entity entity = gameObject.GetComponent<EntityBehaviour>().Entity;
            entity.AddComponent<ChatActiveUserUIComponent>();
            gameObject.transform.SetParent(component, false);
        }

        [OnEventFire]
        public void RemoveWhenUserDisconnected(UserExitFromChatEvent e, UserNode user,
            [JoinByUser] ChatActiveUserUINode chatActiveUserUI) => Object.Destroy(chatActiveUserUI.userLabel.gameObject);

        [OnEventFire]
        public void RemoveUserElementGameObject(NodeRemoveEvent e, ChatActiveUserUINode chatActiveUserUI) =>
            Object.Destroy(chatActiveUserUI.userLabel.gameObject);

        public class UserNode : Node {
            public UserComponent user;

            public UserGroupComponent userGroup;

            public UserUidComponent userUid;
        }

        [Not(typeof(SelfComponent))]
        public class UserNotSelfNode : Node {
            public UserComponent user;

            public UserUidComponent userUid;
        }

        public class ChatActiveUsersNode : Node {
            public ChatActiveUsersGUIComponent chatActiveUsersGui;

            public ScreenGroupComponent screenGroup;
        }

        public class ChatActiveUserUINode : Node {
            public ChatActiveUserUIComponent chatActiveUserUI;
            public UserLabelComponent userLabel;
        }

        public class ChatNode : Node {
            public ChatComponent chat;

            public ChatActiveUserListComponent chatActiveUserList;

            public ChatGroupComponent chatGroup;

            public ScreenGroupComponent screenGroup;
        }
    }
}