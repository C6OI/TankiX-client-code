using System.Collections.Generic;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Battle.ClientHUD.API {
    public class UserNotificatorHUDComponent : BehaviourComponent {
        [SerializeField] UserRankNotificationMessageBehaviour userRankNotificationMessagePrefab;

        [SerializeField] CanvasGroup serviseMessagesCanvasGroup;

        [SerializeField] float serviceMessagesFadeTime = 0.5f;

        BaseUserNotificationMessageBehaviour activeNotification;

        float fadeSpeed;

        Queue<BaseUserNotificationMessageBehaviour> messagesQueue;

        ServiceMessageHUDState serviceMessageState;

        public UserRankNotificationMessageBehaviour UserRankNotificationMessagePrefab => userRankNotificationMessagePrefab;

        void Update() {
            if (serviceMessageState == ServiceMessageHUDState.IDLE) {
                return;
            }

            if (serviceMessageState == ServiceMessageHUDState.FADE_IN) {
                if (serviseMessagesCanvasGroup.alpha >= 1f) {
                    serviseMessagesCanvasGroup.alpha = 1f;
                    serviceMessageState = ServiceMessageHUDState.IDLE;
                } else {
                    serviseMessagesCanvasGroup.alpha += fadeSpeed * Time.deltaTime;
                }
            } else if (serviceMessageState == ServiceMessageHUDState.FADE_OUT) {
                if (serviseMessagesCanvasGroup.alpha <= 0f) {
                    serviseMessagesCanvasGroup.alpha = 0f;
                    serviceMessageState = ServiceMessageHUDState.IDLE;
                    PlayNextNotification();
                } else {
                    serviseMessagesCanvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                }
            }
        }

        void OnEnable() {
            GetComponentsInChildren<BaseUserNotificationMessageBehaviour>(true).ForEach(delegate(BaseUserNotificationMessageBehaviour m) {
                DestroyObject(m.gameObject);
            });

            serviseMessagesCanvasGroup.alpha = 1f;
            serviceMessageState = ServiceMessageHUDState.IDLE;
            fadeSpeed = 1f / serviceMessagesFadeTime;
            messagesQueue = new Queue<BaseUserNotificationMessageBehaviour>();
        }

        void OnUserNotificationFadeOut() {
            activeNotification = null;

            if (messagesQueue.Count > 0) {
                activeNotification = messagesQueue.Dequeue();
                PlayNextNotification();
            } else {
                serviceMessageState = ServiceMessageHUDState.FADE_IN;
            }
        }

        public void Push(BaseUserNotificationMessageBehaviour notification) {
            serviceMessageState = ServiceMessageHUDState.FADE_OUT;

            if (activeNotification == null) {
                activeNotification = notification;
            } else {
                messagesQueue.Enqueue(notification);
            }
        }

        void PlayNextNotification() {
            activeNotification.Play();
        }

        enum ServiceMessageHUDState {
            IDLE = 0,
            FADE_IN = 1,
            FADE_OUT = 2
        }
    }
}