using System;
using Lobby.ClientPayment.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientGarage.Impl;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class AdyenWindow : ECSBehaviour {
        [SerializeField] TextMeshProUGUI info;

        [SerializeField] Animator continueButton;

        [SerializeField] TMP_InputField cardNumber;

        [SerializeField] TMP_InputField mm;

        [SerializeField] TMP_InputField yy;

        [SerializeField] TMP_InputField cvc;

        [SerializeField] TMP_InputField cardHolder;

        Entity item;

        Entity method;

        Action onBack;

        Action onForward;

        void Awake() {
            cardNumber.onValueChanged.AddListener(ValidateInput);
            mm.onValueChanged.AddListener(ValidateInput);
            yy.onValueChanged.AddListener(ValidateInput);
            cvc.onValueChanged.AddListener(ValidateInput);
            cardHolder.onValueChanged.AddListener(ValidateInput);
        }

        void ValidateInput(string value) {
            bool flag = cvc.text.Length >= 3 && BankCardUtils.IsBankCard(cardNumber.text) && yy.text.Length == yy.characterLimit && !string.IsNullOrEmpty(cardHolder.text);

            if (flag) {
                int num = int.Parse(mm.text);
                flag = flag && num >= 1 && num <= 12;
            }

            continueButton.SetBool("Visible", flag);
        }

        public void Show(Entity item, Entity method, Action onBack, Action onForward) {
            this.item = item;
            this.method = method;
            cardNumber.text = string.Empty;
            cvc.text = string.Empty;
            mm.text = string.Empty;
            yy.text = string.Empty;
            cardHolder.text = string.Empty;
            cardNumber.Select();
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
            string publicKey = GetComponent<AdyenPublicKeyComponent>().PublicKey;
            Encrypter encrypter = new(publicKey);

            string encrypedCard = encrypter.Encrypt(new Card {
                number = cardNumber.text.Replace(" ", string.Empty),
                expiryMonth = int.Parse(mm.text).ToString(),
                expiryYear = "20" + yy.text,
                holderName = cardHolder.text,
                cvc = cvc.text
            }.ToString());

            NewEvent(new AdyenBuyGoodsByCardEvent {
                EncrypedCard = encrypedCard
            }).AttachAll(item, method).Schedule();

            onForward();
        }
    }
}