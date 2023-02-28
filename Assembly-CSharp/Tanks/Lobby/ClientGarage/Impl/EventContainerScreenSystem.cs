using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class EventContainerScreenSystem : ECSSystem {
        [OnEventFire]
        public void EventContainerNotificationAdded(NodeAddedEvent e, ContainerNotificationNode node) { }

        [OnEventFire]
        public void Fill(NodeAddedEvent e, EventContainerNotificationNode notification, [JoinAll] UserNode user, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            if (notification.Entity.HasComponent<EventContainerNotificationComponent>()) {
                EventContainerPopupComponent eventContainerPopup = notification.eventContainerPopup;
                eventContainerPopup.itemsContainer.DestroyChildren();
                long containerId = notification.Entity.GetComponent<EventContainerNotificationComponent>().ContainerId;
                EventContainerItemComponent eventContainerItemComponent = Object.Instantiate(eventContainerPopup.itemPrefab, eventContainerPopup.itemsContainer, false);
                Entity entity = Flow.Current.EntityRegistry.GetEntity(containerId);
                eventContainerItemComponent.preview.SpriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                eventContainerItemComponent.text.text = entity.GetComponent<DescriptionItemComponent>().Name;
                eventContainerItemComponent.gameObject.SetActive(true);
                List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
                eventContainerPopup.Show(animators);
            } else {
                ScheduleEvent<NotificationShownEvent>(notification);
            }
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<EventContainerPopupCloseButtonComponent> button, [JoinAll] ICollection<EventContainerNotificationNode> notifications,
            [JoinAll] ICollection<SingleNode<EventContainerPopupComponent>> popups) {
            foreach (SingleNode<EventContainerPopupComponent> popup in popups) {
                popup.component.Hide();
            }

            foreach (EventContainerNotificationNode notification in notifications) {
                ScheduleEvent<NotificationShownEvent>(notification);
            }
        }

        [OnEventFire]
        public void SetRewardInfo(NodeAddedEvent e, EventContainerNotificationNode notification) {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        public class EventContainerNotificationNode : Node {
            public ActiveNotificationComponent activeNotification;

            public EventContainerPopupComponent eventContainerPopup;
        }

        public class ContainerNotificationNode : Node {
            public EventContainerNotificationComponent eventContainerNotification;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}