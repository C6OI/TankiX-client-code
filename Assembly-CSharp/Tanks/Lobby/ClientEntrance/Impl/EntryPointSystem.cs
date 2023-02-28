using System;
using System.IO;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNavigation.Impl;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class EntryPointSystem : ECSSystem {
        public const string AUTO_AUTHENTICATION_TOKEN = "TOToken";

        public const string AUTO_AUTHENTICATION_LOGIN = "TOLogin";

        public const string STEAM_AUTHENTICATION_KEY = "SteamAuthentication";

        [OnEventFire]
        public void SendWebId(NodeAddedEvent e, SingleNode<ClientSessionComponent> node) {
            string text;

            try {
                text = File.ReadAllText(Application.dataPath + "/USER_ID");
                long num = Convert.ToInt64(text);
            } catch (Exception) {
                text = string.Empty;
            }

            ScheduleEvent(new ClientLaunchEvent(text), node);
        }

        [OnEventFire]
        public void CheckAutoLogin(NodeAddedEvent e, SecuredClientSessionNode clientSession, SingleNode<ScreensRegistryComponent> screenRegistry,
            SingleNode<EntranceValidationRulesComponent> validationRules) {
            string @string = PlayerPrefs.GetString("TOLogin");
            string string2 = PlayerPrefs.GetString("TOToken");

            if (!clientSession.Entity.HasComponent<InviteComponent>() && !string.IsNullOrEmpty(string2) && !string.IsNullOrEmpty(@string)) {
                AutoLoginUserEvent autoLoginUserEvent = new();
                autoLoginUserEvent.Uid = @string;
                autoLoginUserEvent.EncryptedToken = PasswordSecurityUtils.RSAEncrypt(clientSession.sessionSecurityPublic.PublicKey, Convert.FromBase64String(string2));
                autoLoginUserEvent.HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint;
                AutoLoginUserEvent eventInstance = autoLoginUserEvent;
                ScheduleEvent(eventInstance, clientSession);
            } else if (IsSteamUserLogin()) {
                ScheduleEvent<ShowFirstScreenEvent<EntranceScreenComponent>>(screenRegistry);
                ScheduleEvent<RequestSteamAuthenticationEvent>(clientSession);
            } else {
                string savedLogin = SaveLoginSystem.GetSavedLogin();

                if (!string.IsNullOrEmpty(savedLogin) || !string.IsNullOrEmpty(clientSession.webId.WebIdUid)) {
                    ScheduleEvent<ShowFirstScreenEvent<EntranceScreenComponent>>(screenRegistry);
                } else {
                    ScheduleEvent<ShowFirstScreenEvent<RegistrationScreenComponent>>(screenRegistry);
                }
            }
        }

        bool IsSteamUserLogin() {
            bool flag = PlayerPrefs.GetInt("SteamAuthentication", 0) == 1;
            return SteamManager.Initialized && !flag;
        }

        [OnEventFire]
        public void ContinueWithLogin(AutoLoginFailedEvent e, Node any, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            ClearAutoLoginToken();
            ScheduleEvent<ShowFirstScreenEvent<EntranceScreenComponent>>(topPanel);
        }

        [Mandatory]
        [OnEventFire]
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

            public WebIdComponent webId;
        }

        public class SessionAwaitingTokenNode : Node {
            public AutoLoginTokenAwaitingComponent autoLoginTokenAwaiting;
            public ClientSessionComponent clientSession;
        }
    }
}