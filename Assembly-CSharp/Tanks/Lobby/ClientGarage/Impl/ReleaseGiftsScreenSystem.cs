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
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ReleaseGiftsScreenSystem : ECSSystem {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, ReleaseGiftsNotificationNode notification, SingleNode<ReleaseGiftsPopup> popup, [JoinAll] UserNode user,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            ReleaseGiftsPopup component = popup.component;
            component.itemsContainer.DestroyChildren();
            int num = 0;

            foreach (KeyValuePair<long, int> item in notification.releaseGiftsNotification.Reward) {
                component.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>().dealy = (num + 1) * component.itemsShowDelay;
                ReleaseGiftItemComponent releaseGiftItemComponent = Object.Instantiate(component.itemPrefab, component.itemsContainer, false);
                Entity entity = Flow.Current.EntityRegistry.GetEntity(item.Key);
                int value = item.Value;
                releaseGiftItemComponent.preview.SpriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                bool flag = value > 1;
                releaseGiftItemComponent.text.text = entity.GetComponent<DescriptionItemComponent>().Name + (!flag ? string.Empty : " x");
                releaseGiftItemComponent.gameObject.SetActive(true);

                if (flag) {
                    releaseGiftItemComponent.count.Value = value;
                } else {
                    releaseGiftItemComponent.count.gameObject.SetActive(false);
                }

                num++;
            }

            List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
            component.Show(animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<ReleaseGiftsPopupCloseButtonComponent> button, [JoinAll] ReleaseGiftsNotificationNode notification,
            [JoinAll] SingleNode<ReleaseGiftsPopup> popup) {
            popup.component.Hide();
            ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventFire]
        public void SetRewardInfo(NodeAddedEvent e, ReleaseGiftsNotificationNode notification) {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        public class ReleaseGiftsNotificationNode : Node {
            public ReleaseGiftsNotificationComponent releaseGiftsNotification;

            public ResourceDataComponent resourceData;
        }

        public class UserNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}