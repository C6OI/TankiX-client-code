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
    public class PrivacyPolicyNotificationSystem : ECSSystem {
        [OnEventFire]
        public void Fill(NodeAddedEvent e, PrivacyPolicyNotificationNode notification, SingleNode<PrivacyPolicyDialog> dialogNode, SingleNode<MainScreenComponent> mainScreen,
            [JoinAll] Optional<SingleNode<WindowsSpaceComponent>> screens) {
            PrivacyPolicyDialog component = dialogNode.component;
            List<Animator> animators = !screens.IsPresent() ? new List<Animator>() : screens.Get().component.Animators;
            component.Show(animators);
        }

        [OnEventFire]
        public void HidePopup(ButtonClickEvent e, SingleNode<PrivacyPolicyOkButton> button, [JoinAll] PrivacyPolicyNotificationNode notification,
            [JoinAll] Optional<SingleNode<PrivacyPolicyDialog>> popup) {
            if (popup.IsPresent()) {
                popup.Get().component.HideByAcceptButton();
            }

            ScheduleEvent<NotificationShownEvent>(notification);
        }

        public class PrivacyPolicyNotificationNode : Node {
            public PrivacyPolicyNotificationComponent privacyPolicyNotification;

            public ResourceDataComponent resourceData;
        }
    }
}