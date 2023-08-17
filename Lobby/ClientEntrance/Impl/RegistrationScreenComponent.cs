using Lobby.ClientEntrance.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientEntrance.Impl {
    public class RegistrationScreenComponent : BehaviourComponent {
        [SerializeField] InputField uidInput;

        [SerializeField] InputField passwordInput;

        [SerializeField] InputField emailInput;

        public virtual string Uid {
            get => uidInput.text;
            set => uidInput.text = value;
        }

        public virtual string Password => passwordInput.text;

        public virtual string Email {
            get => emailInput.text;
            set => emailInput.text = value;
        }

        void Awake() { }

        public InteractivityPrerequisiteComponent GetUidInput() =>
            uidInput.GetComponent<InteractivityPrerequisiteComponent>();

        public InteractivityPrerequisiteComponent GetEmailInput() =>
            emailInput.GetComponent<InteractivityPrerequisiteComponent>();

        public void SetUidInputInteractable(bool interactable) {
            uidInput.interactable = interactable;

            if (interactable) {
                uidInput.GetComponent<Animator>().SetTrigger("Reset");
            } else {
                uidInput.GetComponent<Animator>().SetTrigger("Inactive");
            }
        }

        public void SetEmailInputInteractable(bool interactable) {
            emailInput.interactable = interactable;

            if (interactable) {
                emailInput.GetComponent<Animator>().SetTrigger("Reset");
            } else {
                emailInput.GetComponent<Animator>().SetTrigger("Inactive");
            }
        }
    }
}