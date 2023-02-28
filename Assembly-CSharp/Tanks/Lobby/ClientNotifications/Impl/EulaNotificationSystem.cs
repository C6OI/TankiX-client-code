using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientResources.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNotifications.API;
using Tanks.Lobby.ClientUserProfile.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientNotifications.Impl {
    public class EulaNotificationSystem : ECSSystem {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, [Combine] EulaNotificationNode notification, SingleNode<EulaDialog> dialogNode, SingleNode<MainScreenComponent> mainScreen,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            EulaDialog component = dialogNode.component;
            List<Animator> animators = !screens.IsPresent() ? null : screens.Get().component.Animators;
            component.Show(animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<EulaAgreeButton> button, [JoinAll] EulaNotificationNode notification, [JoinAll] Optional<SingleNode<EulaDialog>> popup) {
            if (popup.IsPresent()) {
                popup.Get().component.HideByAcceptButton();
            }

            ScheduleEvent<NotificationShownEvent>(notification);
        }

        public class EulaNotificationNode : Node {
            public EulaNotificationComponent eulaNotification;

            public ResourceDataComponent resourceData;
        }
    }
}