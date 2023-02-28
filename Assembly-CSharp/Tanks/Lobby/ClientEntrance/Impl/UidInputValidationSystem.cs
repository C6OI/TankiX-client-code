using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class UidInputValidationSystem : InputValidationSystem {
        [OnEventFire]
        public void ValidateUserUid(InputFieldValueChangedEvent e, LoginInputFieldNode loginInputField, [JoinByScreen] UidInputValidationTextNode strings,
            [JoinAll] SingleNode<EntranceValidationRulesComponent> rules) {
            string input = loginInputField.Input;
            UidInputValidationTextComponent uidInputValidationText = strings.uidInputValidationText;

            if (string.IsNullOrEmpty(input)) {
                loginInputField.ToNormalState();
            } else if (!rules.component.IsLoginSymbolsValid(input)) {
                loginInputField.ToInvalidState(uidInputValidationText.LoginContainsRestrictedSymbols);
            } else if (!rules.component.IsLoginBeginingValid(input)) {
                loginInputField.ToInvalidState(uidInputValidationText.LoginBeginsWithSpecialSymbol);
            } else if (rules.component.AreSpecSymbolsTogether(input)) {
                loginInputField.ToInvalidState(uidInputValidationText.LoginContainsSpecialSymbolsInARow);
            } else {
                loginInputField.ToNormalState();
            }
        }

        [OnEventFire]
        public void ValidateUserUid(InputPausedEvent e, LoginInputFieldNode.NormalState loginInputField, [JoinByScreen] UidInputValidationTextNode strings,
            [JoinAll] SingleNode<ClientSessionComponent> clientSession, [JoinAll] SingleNode<EntranceValidationRulesComponent> rules) {
            string input = loginInputField.Input;
            UidInputValidationTextComponent uidInputValidationText = strings.uidInputValidationText;

            if (string.IsNullOrEmpty(input)) {
                loginInputField.ToNormalState();
                return;
            }

            if (!rules.component.IsLoginEndingValid(input)) {
                loginInputField.ToInvalidState(uidInputValidationText.LoginEndsWithSpecialSymbol);
                return;
            }

            if (rules.component.IsLoginTooShort(input)) {
                loginInputField.ToInvalidState(uidInputValidationText.LoginIsTooShort);
                return;
            }

            if (rules.component.IsLoginTooLong(input)) {
                loginInputField.ToInvalidState(uidInputValidationText.LoginIsTooLong);
                return;
            }

            loginInputField.ToAwaitState();
            ScheduleEvent(new CheckUserUidEvent(input), clientSession);
        }

        [OnEventFire]
        public void SetLoginToInvalidState(UserUidOccupiedEvent e, Node any, [JoinAll] LoginInputFieldNode loginInputField, [JoinAll] UidInputValidationTextNode strings) {
            if (e.Uid == loginInputField.Input) {
                loginInputField.ToInvalidState(strings.uidInputValidationText.LoginAlreadyInUse);
            }
        }

        [OnEventFire]
        public void SetLoginToValidState(UserUidVacantEvent e, Node any, [JoinAll] LoginInputFieldNode loginInputField) {
            if (e.Uid == loginInputField.Input) {
                loginInputField.ToValidState();
            }
        }

        public class LoginInputFieldNode : BaseInputFieldNode<RegistrationLoginInputComponent> {
            public class NormalState : LoginInputFieldNode {
                public InputFieldNormalStateComponent inputFieldNormalState;
            }
        }

        public class UidInputValidationTextNode : Node {
            public ScreenGroupComponent screenGroup;
            public UidInputValidationTextComponent uidInputValidationText;
        }
    }
}