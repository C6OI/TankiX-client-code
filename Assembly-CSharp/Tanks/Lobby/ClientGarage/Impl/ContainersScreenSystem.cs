using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ContainersScreenSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void ShowOpenContainerAnimation(OpenVisualContainerEvent e, Node any, [JoinAll] SingleNode<ContainerComponent> containerNode,
            [JoinAll] ICollection<NotificationNode> notifications) {
            containerNode.component.ShowOpenContainerAnimation();

            foreach (NotificationNode notification in notifications) {
                ScheduleEvent<CloseNotificationEvent>(notification);
            }
        }

        [OnEventFire]
        public void OpenContainer(OpenContainerAnimationShownEvent e, Node any, [JoinAll] ContainerNode selectedItem) {
            ScheduleEvent(new OpenSelectedContainerEvent(), GarageItemsRegistry.GetItem<ContainerBoxItem>(selectedItem.marketItemGroup.Key).UserItem);
        }

        [OnEventFire]
        public void UnshareContainer(NodeRemoveEvent e, UserItemNode containerUserItem, [JoinAll] SingleNode<ContainersUI> containerUi) {
            containerUi.component.DeleteContainerItem(containerUserItem.marketItemGroup.Key);
        }

        [OnEventFire]
        public void CloseContainer(CloseNotificationEvent e, SingleNode<NotificationGroupComponent> notification, [JoinAll] SingleNode<ContainerComponent> containerNode) {
            containerNode.component.CloseContainer();
        }

        public class ContainerNode : Node {
            public ContainerMarkerComponent containerMarker;

            public HangarItemPreviewComponent hangarItemPreview;

            public MarketItemGroupComponent marketItemGroup;
        }

        public class NotificationNode : Node {
            public ActiveNotificationComponent activeNotification;
        }

        public class UserItemNode : Node {
            public MarketItemGroupComponent marketItemGroup;
            public UserItemComponent userItem;
        }
    }
}