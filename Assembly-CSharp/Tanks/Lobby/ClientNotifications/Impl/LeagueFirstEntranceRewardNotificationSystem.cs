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
    public class LeagueFirstEntranceRewardNotificationSystem : ECSSystem {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, LeagueFirstEntranceRewardNotificationNode notification, LeagueDialogNode popup, [JoinAll] UserWithLeagueNode user,
            [JoinByLeague] LeagueNode league, [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            PopupDialogComponent popupDialog = popup.popupDialog;
            popupDialog.itemsContainer.DestroyChildren();
            popupDialog.leagueIcon.SpriteUid = league.leagueIcon.SpriteUid;
            popupDialog.leagueIcon.GetComponent<Image>().preserveAspect = true;
            popupDialog.headerText.text = league.leagueEnterNotificationTexts.HeaderText;
            popupDialog.text.text = league.leagueEnterNotificationTexts.Text;
            int num = 0;

            foreach (KeyValuePair<long, int> item in notification.leagueFirstEntranceRewardNotification.Reward) {
                popupDialog.itemPrefab.GetComponent<AnimationTriggerDelayBehaviour>().dealy = (num + 1) * popupDialog.itemsShowDelay;
                LeagueEntranceItemComponent leagueEntranceItemComponent = Object.Instantiate(popupDialog.itemPrefab, popupDialog.itemsContainer, false);
                Entity entity = Flow.Current.EntityRegistry.GetEntity(item.Key);
                int value = item.Value;
                leagueEntranceItemComponent.preview.SpriteUid = entity.GetComponent<ImageItemComponent>().SpriteUid;
                bool flag = value > 1;
                leagueEntranceItemComponent.text.text = entity.GetComponent<DescriptionItemComponent>().Name + (!flag ? string.Empty : " x");
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
        public void HidePopup(ButtonClickEvent e, SingleNode<LeagueEntrancePopupCloseButtonCompoent> button, [JoinAll] [Combine] LeagueFirstEntranceRewardNotificationNode notification,
            [JoinAll] LeagueDialogNode popup) {
            popup.popupDialog.Hide();
            ScheduleEvent<NotificationShownEvent>(notification);
        }

        [OnEventFire]
        public void SetRewardInfo(NodeAddedEvent e, LeagueFirstEntranceRewardNotificationNode notification) {
            notification.Entity.AddComponent(new NotificationMessageComponent(string.Empty));
        }

        public class LeagueFirstEntranceRewardNotificationNode : Node {
            public LeagueFirstEntranceRewardNotificationComponent leagueFirstEntranceRewardNotification;

            public ResourceDataComponent resourceData;
        }

        public class UserWithLeagueNode : Node {
            public LeagueGroupComponent leagueGroup;
            public SelfUserComponent selfUser;

            public UserReputationComponent userReputation;
        }

        public class LeagueNode : Node {
            public ChestBattleRewardComponent chestBattleReward;

            public CurrentSeasonRewardForClientComponent currentSeasonRewardForClient;
            public LeagueComponent league;

            public LeagueConfigComponent leagueConfig;

            public LeagueEnterNotificationTextsComponent leagueEnterNotificationTexts;

            public LeagueGroupComponent leagueGroup;

            public LeagueIconComponent leagueIcon;

            public LeagueNameComponent leagueName;
        }

        public class LeagueDialogNode : Node {
            public LeaguePopupDialogComponent leaguePopupDialog;

            public PopupDialogComponent popupDialog;
        }
    }
}