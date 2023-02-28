using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using Tanks.Lobby.ClientNavigation.Impl;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class EntranceScreenSystem : ECSSystem {
        [OnEventFire]
        public void IntroduceUserToServer(ButtonClickEvent e, SingleNode<LoginButtonComponent> loginButton, [JoinByScreen] SingleNode<EntranceScreenComponent> entranceScreen,
            [JoinAll] ClientSessionRegularNode clientSession) {
            entranceScreen.component.SetWaitIndicator(true);
            entranceScreen.component.LockScreen(true);
            string login = entranceScreen.component.Login;

            if (login.Contains("@")) {
                IntroduceUserByEmailEvent introduceUserByEmailEvent = new();
                introduceUserByEmailEvent.Email = login;
                ScheduleEvent(introduceUserByEmailEvent, clientSession);
            } else {
                IntroduceUserByUidEvent introduceUserByUidEvent = new();
                introduceUserByUidEvent.Uid = login;
                ScheduleEvent(introduceUserByUidEvent, clientSession);
            }
        }

        [OnEventFire]
        public void FillCaptcha(IntroduceUserEvent e, ClientSessionWithCaptchaNode clientSession, [JoinAll] CaptchaInputFieldNode captchaInput) {
            e.Captcha = captchaInput.inputField.Input;
        }

        [OnEventFire]
        public void TurnOffWaitingCover(LoginFailedEvent e, SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen) {
            entranceScreen.component.SetWaitIndicator(false);
            entranceScreen.component.LockScreen(false);
        }

        [OnEventFire]
        public void SendPasswordToServer(PersonalPasscodeEvent e, ClientSessionRegularNode clientSession, [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen) {
            if (e.Passcode.Length == 0) {
                Log.Error("Invalid passcode for user: " + entranceScreen.component.Login);
                return;
            }

            string password = entranceScreen.component.Password;
            bool rememberMe = entranceScreen.component.RememberMe;
            string passwordEncipher = PasswordSecurityUtils.SaltPassword(e.Passcode, password);
            LoginByPasswordEvent loginByPasswordEvent = new();
            loginByPasswordEvent.RememberMe = rememberMe;
            loginByPasswordEvent.PasswordEncipher = passwordEncipher;
            loginByPasswordEvent.HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint;
            LoginByPasswordEvent eventInstance = loginByPasswordEvent;
            ScheduleEvent(eventInstance, clientSession);

            if (rememberMe) {
                AddAwaitingTokenComponent(password, clientSession.Entity);
            }
        }

        [OnEventFire]
        public void ClientLoginSuccessful(NodeAddedEvent e, SelfUserNode selfUser, [JoinByUser] ClientSessionNode clientSessionNode, [JoinAll] SingleNode<TopPanelComponent> topPanel) {
            topPanel.Entity.AddComponent<TopPanelAuthenticatedComponent>();
        }

        [OnEventFire]
        public void EnableCaptcha(NodeAddedEvent e, ClientSessionWithCaptchaNode sessionWithCaptcha, SingleNode<EntranceScreenComponent> entranceScreen) {
            entranceScreen.component.ActivateCaptcha();
        }

        [OnEventFire]
        public void ServerOnlineStatus(NodeAddedEvent e, SingleNode<ClientSessionComponent> clientSession, SingleNode<EntranceScreenComponent> entranceScreen) {
            entranceScreen.component.SetOnlineStatus();
        }

        [OnEventFire]
        public void ServerOfflineStatus(ServerDisconnectedEvent e, SingleNode<EntranceScreenComponent> entranceScreen) {
            entranceScreen.component.SetOfflineStatus();
        }

        [OnEventFire]
        public void RequestNewCaptcha(UpdateCaptchaEvent e, SingleNode<CaptchaComponent> captcha, [JoinAll] SingleNode<ClientSessionComponent> session) {
            ScheduleEvent<ShowCaptchaWaitAnimationEvent>(captcha);
            ScheduleEvent<CaptchaRequestEvent>(session);
        }

        [OnEventFire]
        public void ClearCaptchInput(UpdateCaptchaEvent e, Node captcha, [JoinByScreen] CaptchaInputFieldNode captchInput) {
            captchInput.inputField.Input = string.Empty;
        }

        [OnEventFire]
        public void UpdateCaptchaImage(CaptchaImageEvent e, SingleNode<ClientSessionComponent> session, [JoinAll] SingleNode<EntranceScreenComponent> entranceScreen,
            [JoinByScreen] SingleNode<CaptchaComponent> captcha) {
            captcha.Entity.AddComponent(new CaptchaBytesComponent(e.CaptchaBytes));
        }

        [OnEventFire]
        public void RemoveAutoLoginData(LoginFailedEvent e, SingleNode<AutoLoginTokenAwaitingComponent> clientSession) {
            clientSession.Entity.RemoveComponent<AutoLoginTokenAwaitingComponent>();
        }

        [OnEventFire]
        public void SwitchToRegistration(ButtonClickEvent e, SingleNode<SwitchToRegistrationButtonComponent> node, [JoinAll] ClientSessionWithoutInvitesNode clientSession) {
            ScheduleEvent<ShowScreenDownEvent<RegistrationScreenComponent>>(node);
        }

        void AddAwaitingTokenComponent(string password, Entity clientSession) {
            clientSession.AddComponent(new AutoLoginTokenAwaitingComponent(PasswordSecurityUtils.GetDigest(password)));
        }

        [OnEventFire]
        public void ClientPassRegistration(NodeAddedEvent e, SelfUserNode selfUser, [JoinByUser] ClientSessionNode clientSessionNode) {
            ScheduleEvent<ShowLobbyScreenEvent>(selfUser);
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
            public UserComponent user;

            public UserGroupComponent userGroup;
        }

        public class ClientSessionNode : Node {
            public ClientSessionComponent clientSession;
        }

        public class ClientSessionRegularNode : Node {
            public ClientSessionComponent clientSession;
        }

        public class ClientSessionWithCaptchaNode : Node {
            public CaptchaRequiredComponent captchaRequired;
            public ClientSessionComponent clientSession;
        }

        public class CaptchaInputFieldNode : Node {
            public CaptchaInputFieldComponent captchaInputField;

            public ESMComponent esm;

            public InputFieldComponent inputField;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;
        }

        [Not(typeof(InviteComponent))]
        public class ClientSessionWithoutInvitesNode : Node {
            public ClientSessionComponent clientSession;
        }
    }
}