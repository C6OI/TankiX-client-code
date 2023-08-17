using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientResources.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNotifications.API;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class NewItemNotificationsSystems : ECSSystem {
        [OnEventFire]
        public void NotificationCreated(NodeAddedEvent e, NewUserItemNotifyNode notify) =>
            NewEvent<NewItemNotificationCreatedEvent>().Attach(notify).Attach(notify.itemNotificationUserItem.Entity)
                .Schedule();

        [OnEventFire]
        public void CustomizeNewUserItemNotify(NewItemNotificationCreatedEvent e, NewUserItemNotifyNode notify,
            ImagedUserItemNode item) {
            notify.newIemNotification.Text = item.descriptionItem.name;
            notify.newIemNotification.SetImage(item.imageItem.SpriteUid);
        }

        [OnEventFire]
        public void CustomizeNewUserItemNotify(NewItemNotificationCreatedEvent e, NewUserItemNotifyNode notify,
            NotImagedUserItemNode item, [JoinByParentGroup] DefaultSkinItemNode skin) {
            notify.newIemNotification.Text = item.descriptionItem.name;
            notify.newIemNotification.SetImage(skin.imageItem.SpriteUid);
        }

        public class NewUserItemNotifyNode : Node {
            public ItemNotificationComponent itemNotification;

            public ItemNotificationUserItemComponent itemNotificationUserItem;

            public NewIemNotificationComponent newIemNotification;

            public ResourceDataComponent resourceData;
        }

        public class ImagedUserItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;

            public ImageItemComponent imageItem;
        }

        [Not(typeof(ImageItemComponent))]
        public class NotImagedUserItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;
        }

        public class DefaultSkinItemNode : Node {
            public DefaultSkinItemComponent defaultSkinItem;

            public ImageItemComponent imageItem;

            public MarketItemComponent marketItem;
            public SkinItemComponent skinItem;
        }
    }
}