using System.Collections.Generic;
using Lobby.ClientCommunicator.API;
using Lobby.ClientNavigation.API;
using Platform.Common.ClientECSCommon.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine;

namespace Lobby.ClientCommunicator.Impl {
    public class SectionListSystem : ECSSystem {
        [OnEventFire]
        public void ShowCommunicatorScreen(ButtonClickEvent e, SingleNode<CommunicatorButtonComponent> node) =>
            ScheduleEvent<ShowScreenDownEvent<SectionScreenComponent>>(node);

        [OnEventFire]
        public void AttachToSection(NodeAddedEvent e, SingleNode<SectionScreenComponent> screen,
            [JoinAll] SingleNode<ClientSessionComponent> clientSession) =>
            ScheduleEvent<UserAttachToSectionEvent>(clientSession.Entity);

        [OnEventFire]
        public void AddChatToList(NodeAddedEvent e, [Combine] ChatDescriptionNode node, SectionScreenNode screen,
            [JoinAll] SingleNode<SectionContentGUIComponent> sectionContent) {
            GameObject gameObject = Object.Instantiate(sectionContent.component.SectionAsset);
            RectTransform component = sectionContent.component.gameObject.GetComponent<RectTransform>();
            gameObject.transform.SetParent(component, false);
            SectionListElementComponent component2 = gameObject.GetComponent<SectionListElementComponent>();
            component2.Name = node.name.Name;
            component2.ChatDescriptionEntity = node.Entity;
        }

        [OnEventFire]
        public void RemoveChatFromList(NodeRemoveEvent e, ChatDescriptionNode node,
            [JoinAll] ICollection<SingleNode<SectionListElementComponent>> sectionItems) {
            foreach (SingleNode<SectionListElementComponent> sectionItem in sectionItems) {
                if (Equals(sectionItem.component.ChatDescriptionEntity, node.Entity)) {
                    Object.Destroy(sectionItem.component.gameObject);
                }
            }
        }

        [OnEventFire]
        public void LeaveSection(NodeRemoveEvent e, SingleNode<SectionContentGUIComponent> node,
            [JoinAll] SingleNode<SectionComponent> sectionNode) => ScheduleEvent<UserDetachFromSectionEvent>(sectionNode);

        [OnEventFire]
        public void RequestChat(ButtonClickEvent e, SingleNode<SectionListElementComponent> node) {
            Entity chatDescriptionEntity = node.component.ChatDescriptionEntity;
            ScheduleEvent<CreatePublicChatRequestEvent>(chatDescriptionEntity);
        }

        [OnEventFire]
        public void RemoveElement(NodeRemoveEvent e, SingleNode<SectionListElementComponent> element) =>
            Object.Destroy(element.component.gameObject);

        public class ChatDescriptionNode : Node {
            public ChatDescriptionComponent chatDescription;

            public NameComponent name;

            public PublicChatDescriptionComponent publicChatDescription;
        }

        public class SectionScreenNode : Node {
            public SectionScreenComponent sectionScreen;
        }
    }
}