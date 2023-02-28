using System;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    public class ReceiveMessageSystem : ECSSystem {
        readonly string systemMessageAuthorKey = "ee469a5a-5894-4a5e-8c59-414e614cfb22";

        [OnEventFire]
        public void ShowReceivedMessage(ChatMessageReceivedEvent e, ChatNode chatNode, [JoinAll] SelfUserNode selfNode) {
            if (e.SystemMessage || !selfNode.blackList.BlockedUsers.Contains(e.UserId)) {
                ChatMessage chatMessage = new();
                chatMessage.Author = !e.SystemMessage ? e.UserUid : LocalizationUtils.Localize(systemMessageAuthorKey);
                chatMessage.AvatarId = e.UserAvatarId;
                chatMessage.Message = e.Message;
                chatMessage.Time = DateTime.Now.ToString("HH:mm");
                chatMessage.System = e.SystemMessage;
                chatMessage.Self = e.UserId == selfNode.Entity.Id && !e.SystemMessage;
                chatMessage.ChatType = chatNode.chatChannel.ChatType;
                chatMessage.ChatId = chatNode.Entity.Id;
                ChatMessage message = chatMessage;
                chatNode.chatChannel.AddMessage(message);
                ScheduleEvent(new RecievedLobbyChatMessageEvent(message), chatNode);
            }
        }

        [OnEventFire]
        public void OnRecievedMessage(RecievedLobbyChatMessageEvent e, Node any, [JoinAll] Overall overallChannel) {
            overallChannel.chatChannel.AddMessage(e.Message);
        }

        public class SelfUserNode : Node {
            public BlackListComponent blackList;
            public SelfUserComponent selfUser;

            public UserComponent user;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserUidComponent userUid;
        }

        public class ChatNode : Node {
            public ChatChannelComponent chatChannel;
        }

        public class Overall : ChatNode {
            public OverallChannelComponent overallChannel;
        }
    }
}