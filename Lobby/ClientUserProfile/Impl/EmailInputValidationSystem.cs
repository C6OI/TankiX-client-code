using System;
using Lobby.ClientControls.API;
using Lobby.ClientEntrance.API;
using Lobby.ClientEntrance.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.System.Data.Exchange.ClientNetwork.API;

namespace Lobby.ClientUserProfile.Impl {
    public class EmailInputValidationSystem : InputValidationSystem {
        [OnEventFire]
        public void SetNormalStateWhenEmailInputChanged(InputFieldValueChangedEvent e,
            EmailInputFieldNode emailInputField) => emailInputField.ToNormalState();

        [OnEventFire]
        public void ValidateEmail(InputPausedEvent e, EmailInputFieldNode emailInputField,
            [JoinAll] SingleNode<EntranceValidationRulesComponent> rules,
            [JoinAll] SingleNode<ClientSessionComponent> clientSession) {
            if (string.IsNullOrEmpty(emailInputField.Input)) {
                emailInputField.ToNormalState();
            } else if (rules.component.IsEmailValid(emailInputField.Input)) {
                emailInputField.ToAwaitState();
                ScheduleEvent(new CheckEmailEvent(emailInputField.Input), clientSession);
            } else {
                emailInputField.ToInvalidState(emailInputField.marker.EmailIsInvalid);
            }
        }

        [OnEventFire]
        public void EmailVacant(EmailVacantEvent e, Node node, [JoinAll] EmailInputFieldNode emailInputField) {
            if (string.Equals(e.Email, emailInputField.Input, StringComparison.InvariantCultureIgnoreCase)) {
                if (emailInputField.marker.ExistsIsValid) {
                    emailInputField.ToInvalidState(emailInputField.marker.EmailIsNotConfirmed);
                } else {
                    emailInputField.ToValidState();
                }
            }
        }

        [OnEventFire]
        public void EmailOccupied(EmailOccupiedEvent e, Node node, [JoinAll] EmailInputFieldNode emailInputField) {
            if (string.Equals(e.Email, emailInputField.Input, StringComparison.InvariantCultureIgnoreCase)) {
                if (emailInputField.marker.ExistsIsValid) {
                    emailInputField.ToValidState();
                } else {
                    emailInputField.ToInvalidState(emailInputField.marker.EmailIsOccupied);
                }
            }
        }

        [OnEventFire]
        public void EmailInvalid(EmailInvalidEvent e, Node node, [JoinAll] EmailInputFieldNode emailInputField) {
            if (string.Equals(e.Email, emailInputField.Input, StringComparison.InvariantCultureIgnoreCase)) {
                emailInputField.ToInvalidState(emailInputField.marker.EmailIsInvalid);
            }
        }

        public class EmailInputFieldNode : BaseInputFieldNode<EmailInputFieldComponent> { }
    }
}