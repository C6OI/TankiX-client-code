using System;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Lobby.ClientNavigation.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine;

namespace Lobby.ClientEntrance.Impl {
    public class EntryPointSystem : ECSSystem {
        public const string AUTO_AUTHENTICATION_TOKEN = "TOToken";

        public const string AUTO_AUTHENTICATION_LOGIN = "TOLogin";

        [OnEventFire]
        public void CheckAutoLogin(NodeAddedEvent e, SecuredClientSessionNode clientSession,
            SingleNode<ScreensRegistryComponent> screenRegistry,
            SingleNode<EntranceValidationRulesComponent> validationRules) {
            string @string = PlayerPrefs.GetString("TOLogin");
            string string2 = PlayerPrefs.GetString("TOToken");

            if (string.IsNullOrEmpty(string2) || string.IsNullOrEmpty(@string)) {
                ScheduleEvent<ShowFirstScreenEvent<EntranceScreenComponent>>(screenRegistry);
                return;
            }

            AutoLoginUserEvent autoLoginUserEvent = new();
            autoLoginUserEvent.Uid = @string;

            autoLoginUserEvent.EncryptedToken =
                PasswordSecurityUtils.RSAEncrypt(clientSession.sessionSecurityPublic.PublicKey,
                    Convert.FromBase64String(string2));

            autoLoginUserEvent.HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint;
            ScheduleEvent(autoLoginUserEvent, clientSession);
        }

        [OnEventFire]
        public void ContinueWithLogin(AutoLoginFailedEvent e, Node any, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            ClearAutoLoginToken();
            ScheduleEvent<ShowFirstScreenEvent<EntranceScreenComponent>>(topPanel);
        }

        [OnEventFire]
        [Mandatory]
        public void SaveToken(SaveAutoLoginTokenEvent e, Node user, SessionAwaitingTokenNode clientSession) {
            string value = DecryptToken(clientSession.autoLoginTokenAwaiting.PasswordDigest, e.Token);
            PlayerPrefs.SetString("TOLogin", e.Uid);
            PlayerPrefs.SetString("TOToken", value);
            clientSession.Entity.RemoveComponent<AutoLoginTokenAwaitingComponent>();
        }

        void ClearAutoLoginToken() {
            PlayerPrefs.DeleteKey("TOToken");
            PlayerPrefs.DeleteKey("TOLogin");
        }

        string DecryptToken(byte[] passwordDigest, byte[] encryptedToken) {
            byte[] array = new byte[encryptedToken.Length];

            for (int i = 0; i < encryptedToken.Length; i++) {
                array[i] = (byte)(encryptedToken[i] ^ passwordDigest[i % passwordDigest.Length]);
            }

            return Convert.ToBase64String(array);
        }

        public class SecuredClientSessionNode : Node {
            public ClientSessionComponent clientSession;

            public SessionSecurityPublicComponent sessionSecurityPublic;
        }

        public class SessionAwaitingTokenNode : Node {
            public AutoLoginTokenAwaitingComponent autoLoginTokenAwaiting;
            public ClientSessionComponent clientSession;
        }
    }
}