using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PlatboxWindow : MonoBehaviour {
        [SerializeField] TextMeshProUGUI info;

        [SerializeField] TMP_InputField phone;

        [SerializeField] TextMeshProUGUI code;

        [SerializeField] Animator continueButton;

        Action onBack;

        Action onForward;

        public string EnteredPhoneNumber => code.text + phone.text.Replace(" ", string.Empty);

        void Awake() {
            phone.onValueChanged.AddListener(ValidateInput);
        }

        void ValidateInput(string value) {
            continueButton.SetBool("Visible", phone.text.Length == phone.characterLimit);
        }

        public void Show(Entity item, Entity method, Action onBack, Action onForward) {
            phone.text = string.Empty;
            phone.Select();
            MainScreenComponent.Instance.OverrideOnBack(Cancel);
            this.onBack = onBack;
            this.onForward = onForward;
            gameObject.SetActive(true);
            info.text = ShopDialogs.FormatItem(item, method);
        }

        public void Cancel() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            GetComponent<Animator>().SetTrigger("cancel");
            onBack();
        }

        public void Proceed() {
            MainScreenComponent.Instance.ClearOnBackOverride();
            GetComponent<Animator>().SetTrigger("cancel");
            onForward();
        }
    }
}