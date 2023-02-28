using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.Impl;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class LeagueSeasonEndRewardNotificationSystem : ECSSystem {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, [Combine] LeagueSeasonEndRewardNotificationNode notification, EndSeasonDialogNode popup, [JoinAll] UserWithLeagueNode user,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            PopupDialogComponent popupDialog = popup.popupDialog;
            Entity entity = Flow.Current.EntityRegistry.GetEntity(notification.leagueSeasonEndRewardNotification.LeagueId);
            popupDialog.itemsContainer.DestroyChildren();
            popupDialog.leagueIcon.SpriteUid = entity.GetComponent<LeagueIconComponent>().SpriteUid;
            popupDialog.leagueIcon.GetComponent<Image>().preserveAspect = true;
            int num = 0;

            foreach (KeyValuePair<long, int> item in notification.leagueSeasonEndRewardNotification.Reward) {
                popupDialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>().dealy = (num + 1) * popupDialog.itemsShowDelay;
                LeagueEntranceItemComponent leagueEntranceItemComponent = Object.Instantiate(popupDialog.itemPrefab, popupDialog.itemsContainer, false);
                Entity entity2 = Flow.Current.EntityRegistry.GetEntity(item.Key);
                int value = item.Value;
                leagueEntranceItemComponent.preview.SpriteUid = entity2.GetComponent<ImageItemComponent>().SpriteUid;
                bool flag = value > 1;
                leagueEntranceItemComponent.text.text = entity2.GetComponent<DescriptionItemComponent>().Name + (!flag ? string.Empty : " x");
                leagueEntranceItemComponent.gameObject.SetActive(true);

                if (flag) {
                    leagueEntranceItemComponent.count.Value = value;
                } else {
                    leagueEntranceItemComponent.count.gameObject.SetActive(false);
                }

                num++;
            }

            List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
            popupDialog.Show(animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<LeagueEntrancePopupCloseButtonCompoent> button, [JoinAll] [Combine] LeagueSeasonEndRewardNotificationNode notification,
            [JoinAll] EndSeasonDialogNode popup) {
            popup.popupDialog.Hide();
            ScheduleEvent<NotificationShownEvent>(notification);
        }

        public class LeagueSeasonEndRewardNotificationNode : Node {
            public LeagueSeasonEndRewardNotificationComponent leagueSeasonEndRewardNotification;

            public ResourceDataComponent resourceData;
        }

        public class UserWithLeagueNode : Node {
            public LeagueGroupComponent leagueGroup;
            public SelfUserComponent selfUser;

            public UserReputationComponent userReputation;
        }

        public class EndSeasonDialogNode : Node {
            public EndSeasonPopupDialogComponent endSeasonPopupDialog;

            public PopupDialogComponent popupDialog;
        }
    }
}