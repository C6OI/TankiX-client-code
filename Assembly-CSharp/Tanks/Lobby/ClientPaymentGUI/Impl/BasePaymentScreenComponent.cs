using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class BasePaymentScreenComponent : LocalizedScreenComponent, PaymentScreen {
        [SerializeField] Receipt receipt;

        [SerializeField] Text payButtonLabel;

        [SerializeField] protected Button continueButton;

        public Receipt Receipt => receipt;

        public virtual string PayButtonLabel {
            set => payButtonLabel.text = value;
        }
    }
}