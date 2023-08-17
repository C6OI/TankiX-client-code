using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using UnityEngine;

namespace Lobby.ClientEntrance.Impl {
    public class RegistrationScreenSystem : ECSSystem {
        [OnEventFire]
        public void NavigateLicenseAgreement(ButtonClickEvent e,
            SingleNode<LicenseAgreementLinkComponent> licenseAgreementLink,
            [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings) =>
            NavigateToUrl(strings.component.LicenseAgreementUrl);

        [OnEventFire]
        public void NavigateToGameRules(ButtonClickEvent e, SingleNode<GameRulesLinkComponent> gameRulesLink,
            [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings) =>
            NavigateToUrl(strings.component.GameRulesUrl);

        [OnEventFire]
        public void NavigateToPrivacyPolicy(ButtonClickEvent e, SingleNode<PrivacyPolicyLinkComponent> privacyPolicyLink,
            [JoinAll] SingleNode<RegistrationScreenLocalizedStringsComponent> strings) =>
            NavigateToUrl(strings.component.PrivacyPolicyUrl);

        [OnEventFire]
        public void SwitchToRegistrationCodeEntrance(ButtonClickEvent e,
            SingleNode<RegistrationCodeEntranceLinkComponent> node, [JoinAll] SingleNode<ClientSessionComponent> session) =>
            ScheduleEvent<ShowScreenDownEvent<EnterRegistrationCodeScreenComponent>>(node);

        void NavigateToUrl(string url) => Application.OpenURL(url);

        [OnEventFire]
        public void RegisterNewUser(ButtonClickEvent e, SingleNode<FinishRegistrationButtonComponent> node,
            [JoinByScreen] SingleNode<RegistrationScreenComponent> screen,
            [JoinAll] SecuredClientSessionNode clientSession) {
            RegistrationScreenComponent component = screen.component;
            byte[] digest = PasswordSecurityUtils.GetDigest(component.Password);
            RequestRegisterUserEvent requestRegisterUserEvent = new();
            requestRegisterUserEvent.Uid = component.Uid;
            requestRegisterUserEvent.HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint;

            requestRegisterUserEvent.EncryptedPasswordDigest =
                PasswordSecurityUtils.RSAEncryptAsString(clientSession.sessionSecurityPublic.PublicKey, digest);

            requestRegisterUserEvent.Email = component.Email;
            RequestRegisterUserEvent eventInstance = requestRegisterUserEvent;
            ScheduleEvent(eventInstance, clientSession);
            clientSession.Entity.AddComponent(new AutoLoginTokenAwaitingComponent(digest));
            screen.Entity.AddComponent<LockedScreenComponent>();
        }

        [OnEventFire]
        public void UnlockScreenOnFail(RegistrationFailedEvent e, Node any, [JoinAll] LockedScreenNode screen) =>
            screen.Entity.RemoveComponent<LockedScreenComponent>();

        [OnEventFire]
        public void ClearCheckbox(NodeRemoveEvent e, [Combine] SingleNode<CheckboxComponent> checkbox,
            [JoinByScreen] [Context] SingleNode<RegistrationScreenComponent> registrationScreen) =>
            checkbox.component.IsChecked = false;

        [OnEventComplete]
        public void FillUidField(NodeAddedEvent e, ClientSessionWithUidReservationCodeNode clientSession,
            SingleNode<RegistrationScreenComponent> registrationScreen,
            [JoinByScreen] FinishRegistrationButtonNode finishRegistrationButton) {
            registrationScreen.component.SetUidInputInteractable(false);
            registrationScreen.component.Uid = clientSession.uidReservationCode.Uid;

            if (finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Contains(registrationScreen.component
                    .GetUidInput())) {
                finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Remove(registrationScreen.component
                    .GetUidInput());
            }
        }

        [OnEventComplete]
        public void FillEmailField(NodeAddedEvent e, ClientSessionWithCbqCodeNode clientSession,
            SingleNode<RegistrationScreenComponent> registrationScreen,
            [JoinByScreen] FinishRegistrationButtonNode finishRegistrationButton) {
            registrationScreen.component.SetEmailInputInteractable(false);
            registrationScreen.component.Email = clientSession.cbqCode.Mail;

            if (finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Contains(registrationScreen.component
                    .GetEmailInput())) {
                finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Remove(registrationScreen.component
                    .GetEmailInput());
            }
        }

        [OnEventFire]
        public void UnblockUserInput(NodeAddedEvent e, SingleNode<RegistrationScreenComponent> registrationScreen,
            [JoinByScreen] FinishRegistrationButtonNode finishRegistrationButton) {
            registrationScreen.component.SetUidInputInteractable(true);
            registrationScreen.component.SetEmailInputInteractable(true);

            if (!finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Contains(registrationScreen.component
                    .GetUidInput())) {
                finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Add(registrationScreen.component
                    .GetUidInput());
            }

            if (!finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Contains(registrationScreen.component
                    .GetEmailInput())) {
                finishRegistrationButton.dependentInteractivity.prerequisitesObjects.Add(registrationScreen.component
                    .GetUidInput());
            }
        }

        public class SecuredClientSessionNode : Node {
            public ClientSessionComponent clientSession;

            public SessionSecurityPublicComponent sessionSecurityPublic;
        }

        public class ClientSessionWithUidReservationCodeNode : SecuredClientSessionNode {
            public UidReservationCodeComponent uidReservationCode;
        }

        public class ClientSessionWithCbqCodeNode : SecuredClientSessionNode {
            public CbqCodeComponent cbqCode;
        }

        public class LockedScreenNode : Node {
            public LockedScreenComponent lockedScreen;

            public RegistrationScreenComponent registrationScreen;
        }

        public class FinishRegistrationButtonNode : Node {
            public DependentInteractivityComponent dependentInteractivity;
            public FinishRegistrationButtonComponent finishRegistrationButton;
        }
    }
}