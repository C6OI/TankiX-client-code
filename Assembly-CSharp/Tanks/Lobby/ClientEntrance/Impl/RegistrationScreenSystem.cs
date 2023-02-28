using System.IO;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNavigation.main.csharp.API.Screens;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class RegistrationScreenSystem : ECSSystem {
        [OnEventFire]
        public void NavigateLicenseAgreement(ButtonClickEvent e, SingleNode<LicenseAgreementLinkComponent> licenseAgreementLink,
            [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings) {
            NavigateToUrl(strings.component.LicenseAgreementUrl);
        }

        [OnEventFire]
        public void NavigateToGameRules(ButtonClickEvent e, SingleNode<GameRulesLinkComponent> gameRulesLink, [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings) {
            NavigateToUrl(strings.component.GameRulesUrl);
        }

        [OnEventFire]
        public void NavigateToPrivacyPolicy(ButtonClickEvent e, SingleNode<PrivacyPolicyLinkComponent> privacyPolicyLink,
            [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings) {
            NavigateToUrl(strings.component.PrivacyPolicyUrl);
        }

        void NavigateToUrl(string url) {
            Application.OpenURL(url);
        }

        [OnEventFire]
        public void SwitchToEntrance(ButtonClickEvent e, SingleNode<SwitchToEntranceButtonComponent> node, [JoinAll] SecuredClientSessionNode clientSession,
            [JoinByUser] Optional<SingleNode<UserIncompleteRegistrationComponent>> user) {
            if (!user.IsPresent()) {
                ScheduleEvent<ShowScreenDownEvent<EntranceScreenComponent>>(node);
                return;
            }

            PlayerPrefs.DeleteKey("TOLogin");
            PlayerPrefs.DeleteKey("TOToken");
            PlayerPrefs.SetInt("RemeberMeFlag", 0);
            ScheduleEvent<SwitchToEntranceSceneEvent>(node);
        }

        [OnEventFire]
        public void RegisterNewUser(ButtonClickEvent e, SingleNode<FinishRegistrationButtonComponent> node, [JoinByScreen] SingleNode<RegistrationScreenComponent> screen,
            [JoinAll] SecuredClientSessionNode clientSession, [JoinByUser] Optional<SingleNode<UserIncompleteRegistrationComponent>> user,
            [JoinAll] Optional<SingleNode<SteamMarkerComponent>> steam, [JoinAll] Optional<SubscribeCheckboxNode> subscribeCheckbox) {
            RegistrationScreenComponent component = screen.component;
            byte[] digest = PasswordSecurityUtils.GetDigest(component.Password);
            RequestRegisterUserEvent requestRegisterUserEvent = new();
            requestRegisterUserEvent.Uid = component.Uid;
            requestRegisterUserEvent.HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint;
            requestRegisterUserEvent.EncryptedPasswordDigest = PasswordSecurityUtils.RSAEncryptAsString(clientSession.sessionSecurityPublic.PublicKey, digest);
            requestRegisterUserEvent.Email = component.Email;
            requestRegisterUserEvent.Steam = steam.IsPresent();
            requestRegisterUserEvent.Subscribed = !subscribeCheckbox.IsPresent() || subscribeCheckbox.Get().checkbox.IsChecked;
            RequestRegisterUserEvent eventInstance = requestRegisterUserEvent;
            EventBuilder eventBuilder = NewEvent(eventInstance).Attach(clientSession);

            if (user.IsPresent()) {
                eventBuilder.Attach(user.Get());
            }

            eventBuilder.Schedule();
            clientSession.Entity.AddComponent(new AutoLoginTokenAwaitingComponent(digest));
            screen.component.LockScreen(true);
        }

        [OnEventFire]
        public void SetupQuickRegistration(NodeAddedEvent e, SingleNode<QuickRegistrationButtonComponent> node) {
            node.component.GetComponent<Selectable>().gameObject.SetActive(false);
        }

        [OnEventFire]
        public void SetupBackButtonAndExit(NodeAddedEvent e, SingleNode<RegistrationScreenComponent> registration,
            [JoinAll] Optional<SingleNode<UserIncompleteRegistrationComponent>> user) {
            BackButtonComponent componentInChildren = registration.component.GetComponentInChildren<BackButtonComponent>(true);
            ExitButtonComponent componentInChildren2 = registration.component.GetComponentInChildren<ExitButtonComponent>(true);

            if (user.IsPresent()) {
                if ((bool)componentInChildren) {
                    componentInChildren.gameObject.SetActive(true);
                }

                if ((bool)componentInChildren2) {
                    componentInChildren2.gameObject.SetActive(false);
                }
            } else {
                if ((bool)componentInChildren) {
                    componentInChildren.gameObject.SetActive(false);
                }

                if ((bool)componentInChildren2) {
                    componentInChildren2.gameObject.SetActive(true);
                }
            }
        }

        [OnEventFire]
        public void QuickRegistrationNewUser(ButtonClickEvent e, SingleNode<QuickRegistrationButtonComponent> node, [JoinByScreen] SingleNode<RegistrationScreenComponent> screen,
            [JoinAll] SecuredClientSessionNode clientSession, [JoinAll] Optional<SingleNode<SteamMarkerComponent>> steam, [JoinAll] Optional<SubscribeCheckboxNode> subscribeCheckbox) {
            byte[] digest = PasswordSecurityUtils.GetDigest(Path.GetRandomFileName());
            RequestRegisterUserEvent requestRegisterUserEvent = new();
            requestRegisterUserEvent.Uid = string.Empty;
            requestRegisterUserEvent.Email = string.Empty;
            requestRegisterUserEvent.HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint;
            requestRegisterUserEvent.EncryptedPasswordDigest = PasswordSecurityUtils.RSAEncryptAsString(clientSession.sessionSecurityPublic.PublicKey, digest);
            requestRegisterUserEvent.Steam = steam.IsPresent();
            requestRegisterUserEvent.Subscribed = !subscribeCheckbox.IsPresent() || subscribeCheckbox.Get().checkbox.IsChecked;
            requestRegisterUserEvent.QuickRegistration = true;
            RequestRegisterUserEvent eventInstance = requestRegisterUserEvent;
            ScheduleEvent(eventInstance, clientSession);
            clientSession.Entity.AddComponent(new AutoLoginTokenAwaitingComponent(digest));
            screen.component.LockScreen(true);
        }

        [OnEventFire]
        public void CompleteQuickRegistration(NodeRemoveEvent e, IncompleteRegUserNode user, [JoinAll] SingleNode<RegistrationScreenComponent> screen) {
            PlayerPrefs.SetString("TOLogin", user.userUid.Uid);
        }

        [OnEventFire]
        public void UnlockScreenOnFail(RegistrationFailedEvent e, Node any, SingleNode<RegistrationScreenComponent> screen) {
            screen.component.LockScreen(false);
        }

        [OnEventFire]
        public void ClearCheckbox(NodeRemoveEvent e, [Combine] SingleNode<CheckboxComponent> checkbox,
            [Context] [JoinByScreen] SingleNode<RegistrationScreenComponent> registrationScreen) {
            checkbox.component.IsChecked = false;
        }

        [OnEventFire]
        public void UnblockUserInput(NodeAddedEvent e, SingleNode<RegistrationScreenComponent> registrationScreen, [JoinByScreen] FinishRegistrationButtonNode finishRegistrationButton) {
            registrationScreen.component.SetUidInputInteractable(true);
            registrationScreen.component.SetEmailInputInteractable(true);

            if (!finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Contains(registrationScreen.component.GetUidInput())) {
                finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Add(registrationScreen.component.GetUidInput());
            }

            if (!finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Contains(registrationScreen.component.GetEmailInput())) {
                finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Add(registrationScreen.component.GetUidInput());
            }
        }

        public class SecuredClientSessionNode : Node {
            public ClientSessionComponent clientSession;

            public SessionSecurityPublicComponent sessionSecurityPublic;
        }

        public class SubscribeCheckboxNode : Node {
            public CheckboxComponent checkbox;
            public SubscribeCheckboxComponent subscribeCheckbox;
        }

        public class IncompleteRegUserNode : Node {
            public UserIncompleteRegistrationComponent userIncompleteRegistration;

            public UserUidComponent userUid;
        }

        public class FinishRegistrationButtonNode : Node {
            public DependentInteractivityComponent dependentInteractivity;
            public FinishRegistrationButtonComponent finishRegistrationButton;
        }
    }
}