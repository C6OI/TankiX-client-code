using System.Collections.Generic;
using System.Linq;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class NewItemNotificationSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void RegisterNode(NodeAddedEvent e, MountedChildWithImageNode notification) { }

        [OnEventFire]
        public void CreateMessage(NodeAddedEvent e, NotificationWithoutGroupNode notification) {
            notification.Entity.AddComponent(new NotificationMessageComponent {
                Message = notification.newItemNotificationText.HeaderText
            });
        }

        [OnEventFire]
        public void CreateMessages(ShowNotificationGroupEvent e, SingleNode<NotificationGroupComponent> userItem,
            [JoinBy(typeof(NotificationGroupComponent))]
            ICollection<NotificationWithGroupNode> notificatios) {
            notificatios.ForEach(delegate(NotificationWithGroupNode notification) {
                notification.Entity.AddComponent(new NotificationMessageComponent {
                    Message = notification.newItemNotificationText.HeaderText
                });
            });
        }

        [OnEventFire]
        public void CreateNotification(NodeAddedEvent e, ActiveNotificationNode notification) {
            NewEvent<UpdateNotificationEvent>().Attach(notification).Attach(notification.newItemNotification.Item).Schedule();
        }

        [OnEventFire]
        public void UpdateNotification(UpdateNotificationEvent e, ActiveItemNotificationNode notification, ItemNode item) {
            NewEvent<UpdateNotificationItemNameEvent>().AttachAll(notification, item).Schedule();
            notification.newItemNotificationUnity.HeaderElement.text = notification.newItemNotificationText.HeaderText;
            SetImageOrIcon(notification, item);
        }

        [OnEventFire]
        public void UpdateNotification(UpdateNotificationItemNameEvent e, ActiveItemNotificationNode notification, NotPaintItemNode item) {
            string format = notification.newItemNotificationText.SingleItemText;

            if (notification.newItemNotification.Amount > 1) {
                format = notification.newItemNotificationText.ItemText;
            }

            notification.newItemNotificationUnity.ItemNameElement.text = string.Format(format, item.descriptionItem.Name, notification.newItemNotification.Amount);
            notification.newItemNotificationUnity.SetItemRarity(GarageItemsRegistry.GetItem<GarageItem>(item.Entity));
        }

        [OnEventFire]
        public void UpdateNotification(UpdateNotificationItemNameEvent e, ActiveItemNotificationNode notification, PaintItemNode item) {
            string arg = !item.Entity.HasComponent<TankPaintItemComponent>() ? notification.newPaintItemNotificationText.CoverText : notification.newPaintItemNotificationText.PaintText;
            string singleItemText = notification.newItemNotificationText.SingleItemText;

            notification.newItemNotificationUnity.ItemNameElement.text =
                string.Format(singleItemText, item.descriptionItem.Name + string.Format(" ({0})", arg), notification.newItemNotification.Amount);

            notification.newItemNotificationUnity.SetItemRarity(GarageItemsRegistry.GetItem<GarageItem>(item.Entity));
        }

        [OnEventFire]
        public void UpdateNotification(UpdateNotificationEvent e, ActiveCardNotificationNode notification, ItemNode item, [JoinByParentGroup] Optional<ModuleCardNode> moduleCard,
            [JoinByParentGroup] Optional<NotUserModuleNode> notUserModule, [JoinByParentGroup] [Combine] Optional<UserModuleNode> userModule, [JoinAll] SelfUserNode selfUser) {
            int num = 0;
            int num2 = 0;
            notification.newItemNotificationUnity.ContainerContent = false;

            if (moduleCard.IsPresent()) {
                num = (int)(moduleCard.IsPresent() ? moduleCard.Get().userItemCounter.Count : 0);
                notification.newItemNotificationUnity.view.UpdateView(notUserModule.Get().Entity.Id);

                if (notUserModule.IsPresent() && !userModule.IsPresent()) {
                    num2 = notUserModule.Get().moduleCardsComposition.CraftPrice.Cards;
                }

                if (userModule.IsPresent()) {
                    long num3 = userModule.Get().moduleUpgradeLevel.Level + 1;
                    num2 = num3 <= userModule.Get().moduleCardsComposition.UpgradePrices.Count ? userModule.Get().moduleCardsComposition.UpgradePrices[(int)(num3 - 1)].Cards : 0;
                }

                notification.newItemNotificationUnity.upgradeAnimator.Maximum = num;
                notification.newItemNotificationUnity.upgradeAnimator.StartValue = num - notification.newItemNotification.Amount;
                notification.newItemNotificationUnity.upgradeAnimator.Price = num2;
                notification.newItemNotificationUnity.count = notification.newItemNotification.Amount;
            } else {
                notification.newItemNotificationUnity.view.UpdateViewForCrystal(item.Entity.GetComponent<CardImageItemComponent>().SpriteUid, item.descriptionItem.Name);
                notification.newItemNotificationUnity.upgradeAnimator.Maximum = num;
                notification.newItemNotificationUnity.upgradeAnimator.StartValue = num - notification.newItemNotification.Amount;
                notification.newItemNotificationUnity.upgradeAnimator.Price = num2;
                notification.newItemNotificationUnity.count = notification.newItemNotification.Amount;
            }
        }

        void SetImageOrIcon(ActiveNotificationNode notification, ItemNode item) {
            bool flag = false;
            string text = null;

            if (item.Entity.HasComponent<ImageItemComponent>()) {
                text = item.Entity.GetComponent<ImageItemComponent>().SpriteUid;
            }

            if (text == null) {
                MountedChildWithImageNode mountedChildWithImageNode = Select<MountedChildWithImageNode>(item.Entity, typeof(ParentGroupComponent)).FirstOrDefault();

                if (mountedChildWithImageNode != null) {
                    text = mountedChildWithImageNode.imageItem.SpriteUid;
                }
            }

            if (text == null && item.Entity.HasComponent<ItemIconComponent>()) {
                flag = true;
                text = item.Entity.GetComponent<ItemIconComponent>().SpriteUid;
            }

            if (!string.IsNullOrEmpty(text)) {
                if (!flag) {
                    notification.newItemNotificationUnity.SetItemImage(text);
                } else {
                    notification.newItemNotificationUnity.SetItemIcon(text);
                }
            }
        }

        [OnEventFire]
        public void NewCardsNotificationsClosed(CloseNotificationEvent e, SingleNode<NewCardItemNotificationComponent> cardsNotification,
            [JoinAll] [Combine] SingleNode<NewCardsNotificationClosedTriggerComponent> tutorialTriggerNode) {
            tutorialTriggerNode.component.GetComponent<TutorialShowTriggerComponent>().Triggered();
        }

        [OnEventFire]
        public void NewNotificationsOpened(NodeAddedEvent e, SingleNode<NewItemNotificationComponent> notification, [JoinAll] SingleNode<MainScreenComponent> mainScreeen) {
            mainScreeen.component.CardsCount++;
            mainScreeen.component.NotificationsCount++;
            mainScreeen.component.ShowCardsNotification(true);
            TutorialCanvas.Instance.CardsNotificationsEnabled(true);
        }

        [OnEventFire]
        public void NewCardNotificationsClosed(CloseNotificationEvent e, ActiveCardNotificationNode cards, [JoinAll] SingleNode<MainScreenComponent> mainScreeen) {
            mainScreeen.component.CardsCount--;
            mainScreeen.component.NotificationsCount--;
            mainScreeen.component.HideNewItemNotification();
            TutorialCanvas.Instance.CardsNotificationsEnabled(false);
        }

        [OnEventFire]
        public void NewItemNotificationsClosed(CloseNotificationEvent e, ActiveItemNotificationNode cards, [JoinAll] SingleNode<MainScreenComponent> mainScreeen) {
            mainScreeen.component.NotificationsCount--;
            mainScreeen.component.CardsCount--;
            mainScreeen.component.HideNewItemNotification();
            TutorialCanvas.Instance.CardsNotificationsEnabled(false);
        }

        [OnEventFire]
        public void HideProfile(NodeAddedEvent e, SingleNode<NewItemNotificationComponent> notification, [JoinAll] SingleNode<ProfileScreenComponent> profileScreen) {
            profileScreen.component.HideOnNewItemNotificationShow();
        }

        [OnEventFire]
        public void ShowProfile(CloseNotificationEvent e, ActiveItemNotificationNode notification, [JoinAll] SingleNode<ProfileScreenComponent> profileScreen) {
            profileScreen.component.ShowOnNewItemNotificationCLose();
        }

        public class NotificationNode : Node {
            public NewItemNotificationComponent newItemNotification;

            public NewItemNotificationTextComponent newItemNotificationText;

            public NewPaintItemNotificationTextComponent newPaintItemNotificationText;
        }

        public class ModuleNode : Node {
            public DescriptionItemComponent descriptionItem;

            public ItemBigIconComponent itemBigIcon;

            public ItemIconComponent itemIcon;

            public MarketItemGroupComponent marketItemGroup;

            public ModuleBehaviourTypeComponent moduleBehaviourType;

            public ModuleCardsCompositionComponent moduleCardsComposition;
            public ModuleItemComponent moduleItem;

            public ModuleTankPartComponent moduleTankPart;

            public ModuleTierComponent moduleTier;

            public OrderItemComponent orderItem;
        }

        public class UserModuleNode : ModuleNode {
            public ModuleGroupComponent moduleGroup;

            public ModuleUpgradeLevelComponent moduleUpgradeLevel;

            public UserGroupComponent userGroup;
            public UserItemComponent userItem;
        }

        [Not(typeof(UserItemComponent))]
        public class NotUserModuleNode : ModuleNode { }

        public class MountedChildWithImageNode : Node {
            public ImageItemComponent imageItem;

            public MountedItemComponent mountedItem;

            public SkinItemComponent skinItem;
        }

        [Not(typeof(NotificationGroupComponent))]
        public class NotificationWithoutGroupNode : NotificationNode { }

        public class NotificationWithGroupNode : NotificationNode {
            public NotificationGroupComponent notificationGroup;
        }

        public class ActiveNotificationNode : NotificationNode {
            public ActiveNotificationComponent activeNotification;

            public NewItemNotificationUnityComponent newItemNotificationUnity;
        }

        public class ActiveCardNotificationNode : ActiveNotificationNode {
            public NewCardItemNotificationComponent newCardItemNotification;
        }

        [Not(typeof(NewCardItemNotificationComponent))]
        public class ActiveItemNotificationNode : ActiveNotificationNode { }

        public class ItemNode : Node {
            public DescriptionItemComponent descriptionItem;
            public GarageItemComponent garageItem;
        }

        [Not(typeof(PaintItemComponent))]
        public class NotPaintItemNode : ItemNode { }

        public class PaintItemNode : ItemNode {
            public PaintItemComponent paintItem;
        }

        public class ModuleCardNode : Node {
            public ModuleCardItemComponent moduleCardItem;

            public UserItemComponent userItem;

            public UserItemCounterComponent userItemCounter;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;

            public UserGroupComponent userGroup;
        }

        public class UpdateNotificationEvent : Event { }

        public class UpdateNotificationItemNameEvent : Event { }

        public class InstantiatedNewItemNotification : Node {
            public NewItemNotificationTextComponent newItemNotificationText;

            public NotificationInstanceComponent notificationInstance;
        }
    }
}