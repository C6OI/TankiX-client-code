using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class EnergyCompensationNotificationSystem : ECSSystem {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, EnergyCompensationNotificationNode notification, SingleNode<EnergyCompensationDialog> dialogNode,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            CheckForTutorialEvent checkForTutorialEvent = new();
            ScheduleEvent(checkForTutorialEvent, notification);

            if (checkForTutorialEvent.TutorialIsActive) {
                ScheduleEvent<NotificationShownEvent>(notification);
                return;
            }

            EnergyCompensationDialog component = dialogNode.component;
            List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
            component.Show(notification.energyCompensationNotification.Charges, notification.energyCompensationNotification.Crys, animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<EnergyCompensationDialogCloseButton> button, [JoinAll] EnergyCompensationNotificationNode notification,
            [JoinAll] Optional<SingleNode<EnergyCompensationDialog>> popup) {
            HidePopup(notification, popup);
        }

        [OnEventFire]
        public void HidePopup(ShowTutorialStepEvent e, Node any, [JoinAll] EnergyCompensationNotificationNode notification,
            [JoinAll] Optional<SingleNode<EnergyCompensationDialog>> popup) {
            HidePopup(notification, popup);
        }

        void HidePopup([JoinAll] EnergyCompensationNotificationNode notification, [JoinAll] Optional<SingleNode<EnergyCompensationDialog>> popup) {
            if (popup.IsPresent()) {
                popup.Get().component.Hide();
            }

            ScheduleEvent<NotificationShownEvent>(notification);
        }

        public class EnergyCompensationNotificationNode : Node {
            public EnergyCompensationNotificationComponent energyCompensationNotification;

            public ResourceDataComponent resourceData;
        }
    }
}