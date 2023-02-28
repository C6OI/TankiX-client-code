using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class LoginRewardSystem : ECSSystem {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, SingleNode<LoginRewardDialog> popup, [Combine] LoginRewardsNotificationNode notification, [JoinAll] UserNode user,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            int currentDay = notification.loginRewardsNotification.CurrentDay;
            LoginRewardDialog component = popup.component;

            if (component.allItems.currentDay > currentDay) {
                return;
            }

            component.allItems.Clear();
            int num = 0;

            foreach (KeyValuePair<long, int> item in notification.loginRewardsNotification.Reward) {
                Entity entity = Flow.Current.EntityRegistry.GetEntity(item.Key);

                if (!entity.HasComponent<PremiumQuestItemComponent>()) {
                    component.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>().dealy = (num + 1) * component.itemsShowDelay;
                    ReleaseGiftItemComponent releaseGiftItemComponent = Object.Instantiate(component.itemPrefab, component.itemsContainer, false);
                    component.marketItems.Add(entity);
                    int value = item.Value;
                    releaseGiftItemComponent.preview.SpriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                    bool flag = value > 1;
                    releaseGiftItemComponent.text.text = component.GetRewardItemName(entity);

                    if (entity.HasComponent<PremiumBoostItemComponent>()) {
                        releaseGiftItemComponent.text.text = string.Format(releaseGiftItemComponent.text.text, value);
                        flag = false;
                    }

                    releaseGiftItemComponent.gameObject.SetActive(true);

                    if (flag) {
                        releaseGiftItemComponent.count.Value = value;
                    } else {
                        releaseGiftItemComponent.count.gameObject.SetActive(false);
                    }

                    num++;
                }
            }

            Dictionary<int, List<LoginRewardItem>> dictionary = new();

            foreach (LoginRewardItem item2 in notification.loginRewardsNotification.AllReward) {
                if (dictionary.ContainsKey(item2.Day)) {
                    dictionary[item2.Day].Add(item2);
                    continue;
                }

                dictionary[item2.Day] = new List<LoginRewardItem> { item2 };
            }

            popup.component.allItems.InitItems(dictionary, currentDay);
            List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
            component.Show(animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<LoginRewardAcceptButton> button, [JoinAll] ICollection<LoginRewardsNotificationNode> notifications,
            [JoinAll] SingleNode<LoginRewardDialog> popup) {
            popup.component.Hide();
            ShowGarageCategoryEvent showGarageCategoryEvent = new();

            foreach (Entity marketItem in popup.component.marketItems) {
                if (marketItem.HasComponent<ContainerMarkerComponent>()) {
                    if (marketItem.HasComponent<GameplayChestItemComponent>()) {
                        showGarageCategoryEvent.Category = GarageCategory.BLUEPRINTS;
                    } else {
                        showGarageCategoryEvent.Category = GarageCategory.CONTAINERS;
                    }

                    showGarageCategoryEvent.SelectedItem = marketItem;
                    ScheduleEvent(showGarageCategoryEvent, marketItem);
                    break;
                }

                if (marketItem.HasComponent<WeaponPaintItemComponent>()) {
                    showGarageCategoryEvent.Category = GarageCategory.PAINTS;
                    showGarageCategoryEvent.SelectedItem = marketItem;
                    ScheduleEvent(showGarageCategoryEvent, marketItem);
                    break;
                }

                if (marketItem.HasComponent<PaintItemComponent>()) {
                    showGarageCategoryEvent.Category = GarageCategory.PAINTS;
                    showGarageCategoryEvent.SelectedItem = marketItem;
                    ScheduleEvent(showGarageCategoryEvent, marketItem);
                    break;
                }
            }

            foreach (LoginRewardsNotificationNode notification in notifications) {
                ScheduleEvent<NotificationShownEvent>(notification);
            }
        }

        public class LoginRewardsNotificationNode : Node {
            public LoginRewardsNotificationComponent loginRewardsNotification;

            public ResourceDataComponent resourceData;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}