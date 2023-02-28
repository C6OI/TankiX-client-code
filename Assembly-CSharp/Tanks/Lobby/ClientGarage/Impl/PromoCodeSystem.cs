using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class PromoCodeSystem : ECSSystem {
        static readonly string LINK_PREFIX = "link:";

        [OnEventFire]
        public void InputDisabled(NodeRemoveEvent e, InputFieldNode inputNode) {
            inputNode.inputField.Input = string.Empty;
        }

        [OnEventFire]
        public void InputChanged(InputFieldValueChangedEvent e, InputFieldNode inputField, [JoinAll] SelfUserNode selfUser) {
            inputField.esm.Esm.ChangeState<InputFieldStates.AwaitState>();
            inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
            Log.InfoFormat("InputChanged");
        }

        [OnEventFire]
        public void InputChangedWithDelay(InputPausedEvent e, InputFieldNode inputField, [JoinAll] SelfUserNode selfUser) {
            string text = inputField.inputField.Input.Trim();

            if (!string.IsNullOrEmpty(text)) {
                inputField.esm.Esm.ChangeState<InputFieldStates.AwaitState>();
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
                Log.InfoFormat("InputChangedWithDelay {0}", text);

                ScheduleEvent(new CheckPromoCodeEvent {
                    Code = text
                }, selfUser);
            }
        }

        [OnEventFire]
        public void ActivatePromoCode(ButtonClickEvent e, ActivateButtonNode button, [JoinByScreen] InputFieldNode inputField, [JoinAll] SelfUserNode selfUser) {
            string text = inputField.inputField.Input.Trim();

            if (text.StartsWith(LINK_PREFIX)) {
                Log.InfoFormat("NavigateLink {0}", text);

                ScheduleEvent(new NavigateLinkEvent {
                    Link = text.Substring(LINK_PREFIX.Length)
                }, button);
            } else {
                Log.InfoFormat("ActivatePromoCode {0}", text);

                ScheduleEvent(new ActivatePromoCodeEvent {
                    Code = text
                }, selfUser);

                button.buttonMapping.Button.interactable = false;
                inputField.inputField.Input = string.Empty;
            }
        }

        [OnEventFire]
        public void ShowCheckResult(PromoCodeCheckResultEvent e, SelfUserNode selfUser, [JoinAll] InputFieldNode inputField,
            [JoinAll] SingleNode<PromoCodesScreenLocalizationComponent> promoCodesScreen) {
            string text = inputField.inputField.Input.Trim();
            Log.InfoFormat("ShowCheckResult {0}", e.Result);

            if (text.StartsWith(LINK_PREFIX)) {
                Log.InfoFormat("ShowCheckResult IsLink {0}", text);
                inputField.esm.Esm.ChangeState<InputFieldStates.ValidState>();
                inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
                return;
            }

            inputField.inputField.ErrorMessage = string.IsNullOrEmpty(text) ? string.Empty : promoCodesScreen.component.InputStateTexts[e.Result.ToString()].ToString();

            if (e.Code.Equals(text, StringComparison.OrdinalIgnoreCase)) {
                if (e.Result == PromoCodeCheckResult.VALID) {
                    inputField.esm.Esm.ChangeState<InputFieldStates.ValidState>();
                    inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.AcceptableState>();
                } else {
                    inputField.esm.Esm.ChangeState<InputFieldStates.InvalidState>();
                    inputField.interactivityPrerequisiteESM.Esm.ChangeState<InteractivityPrerequisiteStates.NotAcceptableState>();
                }
            }
        }

        public class InputFieldNode : Node {
            public ESMComponent esm;

            public InputFieldComponent inputField;
            public InteractivityPrerequisiteComponent interactivityPrerequisite;

            public InteractivityPrerequisiteESMComponent interactivityPrerequisiteESM;

            public PromoCodeInputFieldComponent promoCodeInputField;
        }

        public class ActivateButtonNode : Node {
            public ActivatePromocodeButtonComponent activatePromoCodeButton;

            public ButtonMappingComponent buttonMapping;
        }

        public class SelfUserNode : Node {
            public SelfUserComponent selfUser;
        }
    }
}