using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientEntrance.Impl;
using Lobby.ClientNavigation.API;
using Lobby.ClientUserProfile.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;

namespace Lobby.ClientUserProfile.Impl {
    public class EnterNewPasswordScreenSystem : ECSSystem {
        [OnEventFire]
        public void SetUid(NodeAddedEvent e, UidIndicatorNode uidIndicator, [Context] [JoinByScreen] ScreenNode screen,
            [Context] SessionNode session) => uidIndicator.uidIndicator.Uid = session.restorePasswordUserData.Uid;

        [OnEventFire]
        public void ValidatePassword(InputFieldValueChangedEvent e, PasswordInputNode passwordInput,
            [JoinByScreen] SingleNode<UidIndicatorComponent> uid,
            [JoinByScreen] SingleNode<PasswordErrorsComponent> strings) {
            if (passwordInput.inputField.Input == uid.component.Uid) {
                passwordInput.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
                passwordInput.inputField.ErrorMessage = strings.component.PasswordEqualsLogin;

                passwordInput.interactivityPrerequisiteEsm.Esm
                    .ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            }
        }

        [OnEventFire]
        public void Continue(ButtonClickEvent e, SingleNode<ContinueButtonComponent> buton,
            [JoinByScreen] SingleNode<EnterNewPasswordScreenComponent> screen, [JoinByScreen] PasswordInputNode password,
            [JoinAll] SingleNode<SessionSecurityPublicComponent> session) {
            byte[] digest = PasswordSecurityUtils.GetDigest(password.inputField.Input);

            ScheduleEvent(new RequestChangePasswordEvent {
                    HardwareFingerprint = HardwareFingerprintUtils.HardwareFingerprint,
                    PasswordDigest = PasswordSecurityUtils.RSAEncryptAsString(session.component.PublicKey, digest)
                },
                session);

            screen.Entity.AddComponent<LockedScreenComponent>();
            session.Entity.AddComponent(new AutoLoginTokenAwaitingComponent(digest));
        }

        public class UidIndicatorNode : Node {
            public ScreenGroupComponent screenGroup;
            public UidIndicatorComponent uidIndicator;
        }

        public class ScreenNode : Node {
            public EnterNewPasswordScreenComponent enterNewPasswordScreen;

            public ScreenGroupComponent screenGroup;
        }

        public class PasswordInputNode : Node {
            public ESMComponent esm;

            public InputFieldComponent inputField;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteEsm;
            public RegistrationPasswordInputComponent registrationPasswordInput;
        }

        public class SessionNode : Node {
            public RestorePasswordUserDataComponent restorePasswordUserData;
        }
    }
}