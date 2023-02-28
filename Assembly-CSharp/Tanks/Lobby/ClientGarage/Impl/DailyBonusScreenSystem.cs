using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientSettings.API;
using Tanks.Lobby.ClientUserProfile.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DailyBonusScreenSystem : ECSSystem {
        [Inject] public static GarageItemsRegistry GarageItemsRegistry { get; set; }

        [OnEventFire]
        public void InitSounds(NodeAddedEvent e, SoundListenerNode listener, UserReadyForLobbyNode user) {
            if (!(UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>() != null)) {
                DailyBonusSoundsBehaviour dailyBonusSoundsBehaviour = Object.Instantiate(listener.soundListenerResources.Resources.DailyBonusSounds,
                    UISoundEffectController.UITransformRoot.position, UISoundEffectController.UITransformRoot.rotation, UISoundEffectController.UITransformRoot);

                UISoundEffectController.UITransformRoot.gameObject.AddComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour = dailyBonusSoundsBehaviour;
            }
        }

        [OnEventFire]
        public void RemoveSounds(NodeAddedEvent e, UserReadyToBattleNode user) {
            DailyBonusScreenSoundsRoot component = UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>();

            if (!(component == null)) {
                Object.DestroyObject(component.gameObject);
                Object.Destroy(component);
            }
        }

        [OnEventFire]
        public void ResetBtnState(NodeAddedEvent e, DailyBonusMainScreenButtonNode button, UserDailyBonusNode user, DailyBonusConfig dailyBonusConfig) {
            button.dailyBonusMainScreenButton.ResetState();
        }

        [OnEventFire]
        public void UpdateBtnState(UpdateEvent e, DailyBonusMainScreenButtonNode button, [JoinAll] UserDailyBonusNode user, [JoinAll] DailyBonusConfig dailyBonusConfig) {
            UpdateDailyBonus(button, user, dailyBonusConfig);
        }

        void UpdateDailyBonus(DailyBonusMainScreenButtonNode button, UserDailyBonusNode user, DailyBonusConfig dailyBonusConfig) {
            bool flag = Date.Now >= user.userDailyBonusNextReceivingDate.Date;
            bool flag2 = false;

            if (user.userStatistics.Statistics.ContainsKey("BATTLES_PARTICIPATED")) {
                flag2 = user.userStatistics.Statistics["BATTLES_PARTICIPATED"] >= dailyBonusConfig.dailyBonusCommonConfig.BattleCountToUnlockDailyBonuses;
            }

            button.dailyBonusMainScreenButton.IsActiveState = flag && flag2;
            button.dailyBonusMainScreenButton.Interactable = flag2;
        }

        [OnEventFire]
        public void GoToDailyBonusScreen(ButtonClickEvent e, SingleNode<DailyBonusMainScreenButtonComponent> button, [JoinAll] UserDailyBonusNode user,
            [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            if (user.userDailyBonusReceivedRewards.ReceivedRewards.Count == 0 && user.userDailyBonusCycle.CycleNumber == 0L) {
                dialogs.component.Get<DailyBonusInfoScreen>().Show();
            } else {
                dialogs.component.Get<DailyBonusScreenComponent>().Show();
            }
        }

        [OnEventFire]
        public void CloseDailyBonusDialog(ButtonClickEvent e, SingleNode<CloseDailyBonusDialogButtonComponent> button, [JoinAll] SingleNode<Dialogs60Component> dialogs) {
            dialogs.component.Get<DailyBonusScreenComponent>().Hide();
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<DailyBonusScreenComponent> screenNode, [JoinAll] UserDailyBonusNode userDailyBonusNode,
            [JoinAll] DailyBonusConfig dailyBonusConfigNode) {
            screenNode.component.UpdateViewInNextFrame(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void UpdateView(DailyBonusReceivedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode,
            [JoinAll] DailyBonusConfig dailyBonusConfigNode) {
            screenNode.component.UpdateView(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void UpdateView(TargetItemFromDailyBonusReceivedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode,
            [JoinAll] DailyBonusConfig dailyBonusConfigNode) {
            screenNode.component.UpdateView(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void ChangeZone(DailyBonusZoneSwitchedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode,
            [JoinAll] DailyBonusConfig dailyBonusConfigNode) {
            screenNode.component.UpdateView(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void ChangeCycle(DailyBonusCycleSwitchedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode,
            [JoinAll] DailyBonusConfig dailyBonusConfigNode) {
            screenNode.component.UpdateView(userDailyBonusNode, dailyBonusConfigNode);
        }

        [OnEventFire]
        public void OnTakeBonusButtonClick(ButtonClickEvent e, SingleNode<TakeDailyBonusButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen,
            [JoinAll] UserDailyBonusNode userDailyBonusNode) {
            OnTakeBonusButtonClick(screen, userDailyBonusNode);
        }

        [OnEventFire]
        public void OnTakeBonusButtonClick(ButtonClickEvent e, SingleNode<TakeDailyBonusContainerButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen,
            [JoinAll] UserDailyBonusNode userDailyBonusNode) {
            OnTakeBonusButtonClick(screen, userDailyBonusNode);
        }

        void OnTakeBonusButtonClick(SingleNode<DailyBonusScreenComponent> screen, UserDailyBonusNode userDailyBonusNode) {
            MapViewBonusElement selectedBonusElement = screen.component.mapView.selectedBonusElement;

            if (selectedBonusElement == null) {
                throw new Exception("Tried to take a bonus when selected bonus is null");
            }

            screen.component.teleportView.activeTeleportView.GetComponent<AnimationEventListener>().SetPartyHandler(delegate {
                ReceiveDailyBonusEvent eventInstance = new() {
                    Code = selectedBonusElement.dailyBonusData.Code
                };

                ScheduleEvent(eventInstance, userDailyBonusNode);
            });

            screen.component.teleportView.activeTeleportView.GetComponent<Animator>().SetTrigger("party");
            screen.component.DisableAllButtons();
            UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayTake();
        }

        [OnEventFire]
        public void OnTakeDetailTargetButtonClick(ButtonClickEvent e, SingleNode<TakeDailyBonusDetailTargetButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen,
            [JoinAll] UserDailyBonusNode userDailyBonusNode) {
            ReceiveTargetItemFromDetailsByDailyBonusEvent eventInstance = new() {
                DetailMarketItemId = screen.component.completeDetailGarageItem.MarketItemId
            };

            ScheduleEvent(eventInstance, userDailyBonusNode);
            screen.component.DisableAllButtons();
            UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayClick();
        }

        [OnEventFire]
        public void OnUpgradeTeleportButtonClick(ButtonClickEvent e, SingleNode<UpgradeTeleportButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen,
            [JoinAll] UserDailyBonusNode userDailyBonusNode) {
            ScheduleEvent<SwitchDailyBonusZoneEvent>(userDailyBonusNode);
            screen.component.DisableAllButtons();
            UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayUpgrade();
        }

        [OnEventFire]
        public void OnGetNewTeleportButtonClick(ButtonClickEvent e, SingleNode<GetNewTeleportButtonComponent> button, [JoinAll] SingleNode<DailyBonusScreenComponent> screen,
            [JoinAll] UserDailyBonusNode userDailyBonusNode) {
            ScheduleEvent<SwitchDailyBonusCycleEvent>(userDailyBonusNode);
            screen.component.DisableAllButtons();
            UISoundEffectController.UITransformRoot.gameObject.GetComponent<DailyBonusScreenSoundsRoot>().dailyBonusSoundsBehaviour.PlayUpgrade();
        }

        [OnEventFire]
        public void UpdateContainers(TargetItemFromDailyBonusReceivedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode,
            [JoinAll] ICollection<SingleNode<ContainersUI>> containersUis) {
            containersUis.ForEach(delegate(SingleNode<ContainersUI> c) {
                c.component.UpdateContainerUI();
            });

            screenNode.component.Hide();
            ContainerBoxItem containerBoxItem = GarageItemsRegistry.GetItem<DetailItem>(e.DetailMarketItemId).TargetMarketItem as ContainerBoxItem;

            ScheduleEvent(new ShowGarageCategoryEvent {
                Category = GarageCategory.CONTAINERS,
                SelectedItem = containerBoxItem.MarketItem
            }, userDailyBonusNode);
        }

        [OnEventFire]
        public void UpdateContainers(DailyBonusReceivedEvent e, UserDailyBonusNode userDailyBonusNode, [JoinAll] SingleNode<DailyBonusScreenComponent> screenNode,
            [JoinAll] DailyBonusConfig dailyBonusConfigNode, [JoinAll] ICollection<SingleNode<ContainersUI>> containersUis) {
            DailyBonusData dailyBonusData = screenNode.component.GetCycle(userDailyBonusNode, dailyBonusConfigNode).DailyBonuses.Where(d => d.Code == e.Code).First();

            if (dailyBonusData.ContainerReward != null) {
                containersUis.ForEach(delegate(SingleNode<ContainersUI> c) {
                    c.component.UpdateContainerUI();
                });

                screenNode.component.Hide();
                ContainerBoxItem item = GarageItemsRegistry.GetItem<ContainerBoxItem>(dailyBonusData.ContainerReward.MarketItemId);

                ScheduleEvent(new ShowGarageCategoryEvent {
                    Category = !item.IsBlueprint ? GarageCategory.CONTAINERS : GarageCategory.BLUEPRINTS,
                    SelectedItem = item.MarketItem
                }, userDailyBonusNode);
            }
        }

        public class UserDailyBonusNode : SelfUserNode {
            public UserDailyBonusCycleComponent userDailyBonusCycle;
            public UserDailyBonusInitializedComponent userDailyBonusInitialized;

            public UserDailyBonusNextReceivingDateComponent userDailyBonusNextReceivingDate;

            public UserDailyBonusReceivedRewardsComponent userDailyBonusReceivedRewards;

            public UserDailyBonusZoneComponent userDailyBonusZone;

            public UserStatisticsComponent userStatistics;
        }

        public class DailyBonusConfig : Node {
            public DailyBonusCommonConfigComponent dailyBonusCommonConfig;

            public DailyBonusEndlessCycleComponent dailyBonusEndlessCycle;

            public DailyBonusFirstCycleComponent dailyBonusFirstCycle;
        }

        public class DailyBonusMainScreenButtonNode : Node {
            public DailyBonusMainScreenButtonComponent dailyBonusMainScreenButton;
        }

        public class SoundListenerNode : Node {
            public SoundListenerComponent soundListener;

            public SoundListenerResourcesComponent soundListenerResources;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }

        public class UserReadyForLobbyNode : SelfUserNode {
            public UserReadyForLobbyComponent userReadyForLobby;
        }

        public class UserReadyToBattleNode : Node {
            public SelfBattleUserComponent selfBattleUser;

            public UserReadyToBattleComponent userReadyToBattle;
        }
    }
}