using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientEntrance.API;
using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class RegistrationScreenComponent : BehaviourComponent, NoScaleScreen {
        [SerializeField] TMP_InputField uidInput;

        [SerializeField] TMP_InputField passwordInput;

        [SerializeField] TMP_InputField emailInput;

        public GameObject locker;

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

        void OnEnable() {
            LockScreen(false);
        }

        public InteractivityPrerequisiteComponent GetUidInput() => uidInput.GetComponent<InteractivityPrerequisiteComponent>();

        public InteractivityPrerequisiteComponent GetEmailInput() => emailInput.GetComponent<InteractivityPrerequisiteComponent>();

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

        public void LockScreen(bool value) {
            locker.SetActive(value);
        }
    }
}