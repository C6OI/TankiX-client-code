using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientCommunicator.API;
using Tanks.Lobby.ClientEntrance.API;
using UnityEngine;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class CreateChatSystem : ECSSystem {
        public EntityBehaviour CreateChat(Entity entity, ChatType chatType, SingleNode<ChatDialogComponent> dialog, bool select = true) {
            EntityBehaviour entityBehaviour = dialog.component.CreateChatChannel(chatType);
            entityBehaviour.BuildEntity(entity);

            if (!entityBehaviour.Entity.HasComponent<ChatChannelComponent>()) {
                entityBehaviour.Entity.AddComponent(new ChatChannelComponent(chatType));
            }

            if (select) {
                ScheduleEvent(new SelectChannelEvent(), entity);
            }

            return entityBehaviour;
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, SingleNode<GeneralChatComponent> chat, SingleNode<ChatDialogComponent> dialog) {
            CreateChat(chat.Entity, chat.component.ChatType, dialog);
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, SingleNode<OverallChannelComponent> chat, SingleNode<ChatDialogComponent> dialog) {
            CreateChat(chat.Entity, chat.component.ChatType, dialog, false);
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, SingleNode<CustomChatComponent> chat, SingleNode<ChatDialogComponent> dialog) {
            CreateChat(chat.Entity, chat.component.ChatType, dialog);
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, SingleNode<SquadChatComponent> chat, SingleNode<ChatDialogComponent> dialog) {
            CreateChat(chat.Entity, chat.component.ChatType, dialog);
        }

        [OnEventComplete]
        public void CreateChat(NodeAddedEvent e, [Combine] PersonalChatNode chat, SingleNode<ChatDialogComponent> dialog, [JoinAll] SelfUser selfUser) {
            if (selfUser.personalChatOwner.ChatsIs.Contains(chat.Entity.Id)) {
                CreatePersonalChat(chat, selfUser, dialog);
            } else if (!chat.Entity.HasComponent<ChatChannelComponent>()) {
                chat.Entity.AddComponent(new ChatChannelComponent(chat.personalChat.ChatType));
            }
        }

        [OnEventFire]
        public void OpenPersonalChannel(OpenPersonalChannelEvent e, SelfUser selfUser, Optional<SingleNode<ChatComponent>> contextChat, [JoinAll] ICollection<PersonalChatNode> chats,
            [JoinAll] SingleNode<ChatDialogComponent> dialog) {
            PersonalChatNode personalChatNode = chats.FirstOrDefault(chat =>
                chat.chatParticipants.Users.Any(user => user.HasComponent<UserComponent>() && user.GetComponent<UserUidComponent>().Uid.Equals(e.UserUid)));

            if (personalChatNode != null) {
                if (personalChatNode.Entity.HasComponent<ChatChannelUIComponent>()) {
                    ScheduleEvent(new SelectChannelEvent(), personalChatNode);
                } else {
                    CreatePersonalChat(personalChatNode, selfUser, dialog);
                }

                return;
            }

            EventBuilder eventBuilder = NewEvent(new CreatePrivateChatEvent {
                UserUid = e.UserUid
            });

            eventBuilder.Attach(selfUser);

            if (contextChat.IsPresent()) {
                eventBuilder.Attach(contextChat.Get());
            }

            eventBuilder.Schedule();
        }

        [OnEventFire]
        public void CreatePersonalChannelOnRecievedMessage(RecievedLobbyChatMessageEvent e, HiddenPersonalChatNode channel, [JoinAll] SingleNode<ChatDialogComponent> dialog,
            [JoinAll] SelfUser selfUser) {
            if (!e.Message.System) {
                CreatePersonalChat(channel, selfUser, dialog, false);
            }
        }

        void CreatePersonalChat(PersonalChatNode chat, SelfUser selfUser, SingleNode<ChatDialogComponent> dialog, bool select = true) {
            EntityBehaviour entityBehaviour = CreateChat(chat.Entity, chat.personalChat.ChatType, dialog, false);
            Entity entity = chat.chatParticipants.Users.FirstOrDefault(x => !ReferenceEquals(x, selfUser.Entity));

            if (entity == null) {
                Debug.LogWarningFormat("CreatePersonalChat self {0}, other = null", selfUser);
                return;
            }

            if (!entity.HasComponent<UserAvatarComponent>()) {
                Debug.LogWarningFormat("CreatePersonalChat self {0}, other {1} = avatar not found", selfUser, entity);
                return;
            }

            string id = entity.GetComponent<UserAvatarComponent>().Id;
            entityBehaviour.GetComponent<ChatChannelUIComponent>().SetIcon(id);

            if (select) {
                ScheduleEvent(new SelectChannelEvent(), entityBehaviour.GetComponent<EntityBehaviour>().Entity);
            }

            Debug.LogFormat("chat user id: {0}", chat.chatParticipants.Users.First(x => !x.Id.Equals(selfUser.Entity.Id)).Id);
        }

        [OnEventComplete]
        public void SetPersonalChatName(NodeAddedEvent e, [Combine] UserNode user, [Combine] PersonalChatUINode chat, [JoinAll] SingleNode<ChatDialogComponent> dialog,
            [JoinAll] SelfUser selfUser) {
            if (!selfUser.Entity.Id.Equals(user.Entity.Id) && chat.chatParticipants.Users.Contains(user.Entity)) {
                dialog.component.ChangeName(chat.chatChannelUI.gameObject, chat.personalChat.ChatType, user.userUid.Uid);

                if (chat.Entity.HasComponent<ActiveChannelComponent>()) {
                    dialog.component.SetHeader(user.userAvatar.Id, chat.chatChannelUI.Name, true);
                }
            }
        }

        [OnEventComplete]
        public void onRemoveChat(NodeRemoveEvent e, VisibleNotGeneralChatNode chat, [JoinAll] SingleNode<ChatDialogComponent> dialog, [JoinAll] SingleNode<GeneralChatComponent> general) {
            GameObject tab = chat.chatChannelUI.Tab;

            if (tab != null) {
                Object.Destroy(tab);
            }
        }

        [OnEventComplete]
        public void RemovePersonal(NodeRemoveEvent e, NotGeneralChatNode chat, [JoinAll] SingleNode<ChatDialogComponent> dialog, [JoinAll] GeneralChatUINode general) {
            if (!general.Entity.HasComponent<ActiveChannelComponent>() && chat.Entity.HasComponent<ActiveChannelComponent>()) {
                ScheduleEvent(new SelectChannelEvent(), general);
            }
        }

        [OnEventComplete]
        public void CloseChannel(CloseChannelEvent e, ActivePersonalChatNode channel, [JoinAll] SingleNode<GeneralChatComponent> general) {
            ScheduleEvent(new SelectChannelEvent(), general);
        }

        [OnEventComplete]
        public void CloseChannel(CloseChannelEvent e, VisiblePersonalChatNode channel, [JoinAll] SingleNode<GeneralChatComponent> general) {
            GameObject tab = channel.chatChannelUI.Tab;

            if (tab != null) {
                Object.DestroyImmediate(tab);
            }
        }

        public class UserNode : Node {
            public UserAvatarComponent userAvatar;
            public UserUidComponent userUid;
        }

        public class GeneralChatUINode : Node {
            public ChatChannelUIComponent chatChannelUI;
            public GeneralChatComponent generalChat;
        }

        public class PersonalChatNode : Node {
            public ChatParticipantsComponent chatParticipants;
            public PersonalChatComponent personalChat;
        }

        public class NotGeneralChatNode : Node {
            public ChatParticipantsComponent chatParticipants;
        }

        public class PersonalChatUINode : PersonalChatNode {
            public ChatChannelUIComponent chatChannelUI;
        }

        [Not(typeof(ChatChannelUIComponent))]
        public class HiddenPersonalChatNode : PersonalChatNode {
            public ChatChannelComponent chatChannel;
        }

        public class VisiblePersonalChatNode : PersonalChatNode {
            public ChatChannelUIComponent chatChannelUI;
        }

        public class ActivePersonalChatNode : PersonalChatNode {
            public ActiveChannelComponent activeChannel;
        }

        public class VisibleNotGeneralChatNode : NotGeneralChatNode {
            public ChatChannelUIComponent chatChannelUI;
        }

        public class ActiveNotGeneralChatNode : NotGeneralChatNode {
            public ActiveChannelComponent activeChannel;
        }

        public class SelfUser : Node {
            public PersonalChatOwnerComponent personalChatOwner;
            public SelfUserComponent selfUser;

            public UserUidComponent userUid;
        }
    }
}