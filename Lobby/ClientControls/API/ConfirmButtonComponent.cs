using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Component = Platform.Kernel.ECS.ClientEntitySystem.API.Component;

namespace Lobby.ClientControls.API {
    [RequireComponent(typeof(Animator))]
    public class ConfirmButtonComponent : MonoBehaviour, Component {
        [SerializeField] public Button button;

        [SerializeField] Text buttonText;

        [SerializeField] Text confirmText;

        [SerializeField] Text cancelText;

        [SerializeField] Button defaultButton;

        public bool EnableOutsideClicking { get; private set; }

        public string ButtonText {
            get => buttonText.text;
            set => buttonText.text = value;
        }

        public string ConfirmText {
            get => confirmText.text;
            set => confirmText.text = value;
        }

        public string CancelText {
            get => cancelText.text;
            set => cancelText.text = value;
        }

        public void FlipFront() {
            GetComponent<Animator>().SetBool("flip", false);
            StartCoroutine(DelayActivation(button));
        }

        public void FlipBack() {
            GetComponent<Animator>().SetBool("flip", true);
            StartCoroutine(DelayActivation(defaultButton));
        }

        public void Confirm() => GetComponent<Animator>().SetTrigger("confirm");

        IEnumerator DelayActivation(Button button) {
            while (!button.isActiveAndEnabled) {
                yield return new WaitForEndOfFrame();
            }

            button.Select();
        }

        void DisableOutsideClickingOption() => EnableOutsideClicking = false;

        void EnableOutsideClickingOption() => EnableOutsideClicking = true;
    }
}