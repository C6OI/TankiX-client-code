using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class FirstPurchaseConfirmScreenComponent : LocalizedScreenComponent, PaymentScreen {
        [SerializeField] Text info;

        [SerializeField] Text confirmButton;

        [SerializeField] PaletteColorField color;

        [SerializeField] GameObject overlay;

        [SerializeField] CanvasGroup content;

        long compensation;

        public string ConfirmationText { private get; set; }

        public long Compensation {
            get => compensation;
            set {
                compensation = value;
                info.text = string.Format(ConfirmationText, string.Format("<color=#{0}>{1}</color>", color.Color.ToHexString(), compensation));
                content.interactable = true;
                overlay.SetActive(false);
            }
        }

        public string ConfirmButton {
            set => confirmButton.text = value;
        }

        void OnDisable() {
            info.text = string.Empty;
            content.interactable = false;
            overlay.SetActive(true);
        }
    }
}