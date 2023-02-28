using System;
using System.Collections.Generic;
using System.Linq;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.Impl {
    public class WarmingUpTimerNotificationsComponent : BehaviourComponent {
        [SerializeField] List<NotificationTime> warmingUpTimerNotifications;

        [SerializeField] GameObject startBattleNotification;

        Queue<NotificationTime> notifications = new();

        bool notificationsInitialized;

        public float NextNotificationTime { get; set; }

        void OnDisable() {
            SetNotificationsToInactiveState();
        }

        public void Init(float remainingTime) {
            SetNotificationsToInactiveState();
            notifications = new Queue<NotificationTime>(warmingUpTimerNotifications.Where(notification => notification.remainingTime <= remainingTime));
            NextNotificationTime = GetNextNotificationTime();
            notificationsInitialized = true;
        }

        public void ShowNextNotification() {
            if (notificationsInitialized) {
                notifications.Dequeue().notification.SetActive(true);
                NextNotificationTime = GetNextNotificationTime();
            }
        }

        public void ShowStartBattleNotification() {
            startBattleNotification.SetActive(true);
        }

        public bool HasNotifications() => notifications.Count > 0;

        public float GetNextNotificationTime() => !HasNotifications() ? -1f : notifications.Peek().remainingTime;

        void SetNotificationsToInactiveState() {
            EnumerableExtension.ForEach(warmingUpTimerNotifications, delegate(NotificationTime notification) {
                notification.notification.SetActive(false);
            });

            startBattleNotification.SetActive(false);
        }

        public void DeactivateNotifications() {
            notificationsInitialized = false;
        }

        [Serializable]
        public class NotificationTime {
            public float remainingTime;

            public GameObject notification;
        }
    }
}