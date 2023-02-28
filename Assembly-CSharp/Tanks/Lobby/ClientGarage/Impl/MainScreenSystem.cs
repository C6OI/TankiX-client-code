using System.Collections.Generic;
using System.Globalization;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientSettings.API;
using UnityEngine;
using Event = UnityEngine.Event;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MainScreenSystem : ECSSystem {
        [OnEventFire]
        public void GoHome(ButtonClickEvent e, SingleNode<HomeButtonComponent> node) {
            MainScreenComponent.Instance.ClearHistory();
            ScheduleEvent<ShowScreenDownEvent<MainScreenComponent>>(node);
        }

        [OnEventFire]
        public void FinalizeCompactWindow(NodeAddedEvent e, SingleNode<MainScreenComponent> homeScreen) {
            GraphicsSettings.INSTANCE.DisableCompactScreen();
        }

        [OnEventFire]
        public void GoHomeByKeyHome(UpdateEvent e, SingleNode<HomeButtonComponent> node) {
            if (Input.GetKeyDown(KeyCode.Home) && !InputFieldComponent.IsAnyInputFieldInFocus()) {
                MainScreenComponent.Instance.ClearHistory();
                ScheduleEvent<ShowScreenDownEvent<MainScreenComponent>>(node);
            }
        }

        [OnEventFire]
        public void GoHomeByKeyHome(UpdateEvent e, SingleNode<MainScreenComponent> node) {
            if (Input.GetKeyDown(KeyCode.Home) && !InputFieldComponent.IsAnyInputFieldInFocus()) {
                node.component.ShowHome();
            }
        }

        [OnEventFire]
        public void GoBackByKey(UpdateEvent e, SingleNode<MainScreenComponent> node, [JoinAll] ICollection<CancelNotificationNode> cancelNotifications) {
            if (Event.current != null && Event.current.isMouse) {
                ScheduleEvent<HangarCameraDelayAutoRotateEvent>(node);
            }

            if (InputMapping.Cancel && !InputFieldComponent.IsAnyInputFieldInFocus() && node.component.HasHistory() && cancelNotifications.Count == 0 &&
                TutorialCanvas.Instance.AllowCancelHandler) {
                node.component.GoBack();
            }
        }

        [OnEventFire]
        public void OnMain(NodeAddedEvent e, SingleNode<MainScreenComponent> screen, SelfUserNode selfUser) {
            if (!selfUser.Entity.HasComponent<UserCountryComponent>()) {
                string countryCode = RegionInfo.CurrentRegion == null ? "US" : RegionInfo.CurrentRegion.Name;
                ConfirmUserCountryEvent confirmUserCountryEvent = new();
                confirmUserCountryEvent.CountryCode = countryCode;
                ConfirmUserCountryEvent eventInstance = confirmUserCountryEvent;
                ScheduleEvent(eventInstance, selfUser);
            }

            screen.component.ShowLast();
            ScheduleEvent<SetScreenHeaderEvent>(screen);
        }

        [OnEventFire]
        public void OnMain(NodeRemoveEvent e, SingleNode<MainScreenComponent> screen) {
            ScheduleEvent<ResetPreviewEvent>(screen);
        }

        [OnEventFire]
        public void HoldTheDoor(ItemUpgradeUpdatedEvent e, Node any) { }

        [OnEventFire]
        public void HoldTheDoor(NodeAddedEvent e, SingleNode<UpgradeMaxLevelItemComponent> any) { }

        [OnEventFire]
        public void RefreshItemSelectUI(UpdateRankEvent evt, Node node, [JoinAll] SingleNode<MainScreenComponent> screen) {
            ItemSelectUI itemSelectUI = Object.FindObjectOfType<ItemSelectUI>();

            if ((bool)itemSelectUI) {
                itemSelectUI.RefreshSelection();
            }
        }

        [OnEventFire]
        public void OpenExitDialog(ButtonClickEvent e, SingleNode<QuitButtonComponent> quitButton, [JoinAll] SingleNode<Dialogs60Component> dialogs,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            List<Animator> animators = new();

            if (screens.IsPresent()) {
                animators = screens.Get().component.Animators;
            }

            ExitGameDialog exitGameDialog = dialogs.component.Get<ExitGameDialog>();
            exitGameDialog.Show(animators);
        }

        [OnEventFire]
        public void Exit(DialogConfirmEvent e, SingleNode<ExitGameDialog> exitDialog) {
            Application.Quit();
        }

        [OnEventFire]
        public void FillWindowsSpace(NodeAddedEvent e, SingleNode<WindowsSpaceComponent> space, SingleNode<WindowsSpaceFillComponent> fill) {
            space.component.Animators.AddRange(fill.component.Animators);
        }

        public class CancelNotificationNode : Node {
            public CancelNotificationComponent cancelNotification;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}