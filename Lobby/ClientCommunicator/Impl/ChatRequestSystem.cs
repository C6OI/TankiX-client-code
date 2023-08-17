using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Common.ClientECSCommon.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientCommunicator.Impl {
    public class ChatRequestSystem : ECSSystem {
        [OnEventFire]
        public void CreateChatRequest(CreatePublicChatRequestEvent e, PublicChatDescripionNode chatDescription) {
            Entity entity = CreateEntity("PublicChatRequest_" + chatDescription.Entity.Id);
            entity.AddComponent(new PublicChatRequestComponent(chatDescription.Entity.Id, chatDescription.name.Name));
            ShowScreenLeftEvent<ChatScreenComponent> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(entity, true);
            ScheduleEvent(showScreenLeftEvent, entity);
        }

        [OnEventFire]
        public void AttachToChatOnRequest(NodeAddedEvent e, PublicChatRequestOnScreenNode requestNode,
            [JoinAll] SingleNode<ClientSessionComponent> clientSession) => ScheduleEvent(
            new RequestEnterToPublicChatEvent(requestNode.publicChatRequest.ChatDescripionId),
            clientSession);

        [OnEventFire]
        public void CreateChatRequest(CreatePrivateChatRequestEvent e, UserNode user) {
            Entity entity = CreateEntity("PrivateChatRequest_" + user.userUid.Uid);
            entity.AddComponent(new PrivateChatRequestComponent(user.Entity.Id, user.userUid.Uid));
            ShowScreenLeftEvent<ChatScreenComponent> showScreenLeftEvent = new();
            showScreenLeftEvent.SetContext(entity, true);
            ScheduleEvent(showScreenLeftEvent, entity);
        }

        [OnEventFire]
        public void AttachToChatOnRequest(NodeAddedEvent e, PrivateChatRequestOnScreenNode requestNode,
            [JoinAll] SingleNode<ClientSessionComponent> clientSession) =>
            ScheduleEvent(new RequestEnterToPrivateChatEvent(requestNode.privateChatRequest.UserId), clientSession);

        public class PrivateChatRequestOnScreenNode : Node {
            public PrivateChatRequestComponent privateChatRequest;

            public ScreenGroupComponent screenGroup;
        }

        public class PublicChatRequestOnScreenNode : Node {
            public PublicChatRequestComponent publicChatRequest;

            public ScreenGroupComponent screenGroup;
        }

        public class PublicChatDescripionNode : Node {
            public ChatDescriptionComponent chatDescription;

            public NameComponent name;

            public PublicChatDescriptionComponent publicChatDescription;
        }

        public class UserNode : Node {
            public UserComponent user;

            public UserUidComponent userUid;
        }
    }
}