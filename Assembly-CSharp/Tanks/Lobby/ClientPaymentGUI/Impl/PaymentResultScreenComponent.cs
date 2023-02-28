using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PaymentResultScreenComponent : LocalizedScreenComponent, PaymentScreen {
        [SerializeField] Text message;

        public virtual string Message {
            set => message.text = value;
        }
    }
}