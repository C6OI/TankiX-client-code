using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class MobilePhoneInputComponent : BehaviourComponent {
        [SerializeField] Text phoneCountryCode;

        public virtual string PhoneCountryCode {
            set => phoneCountryCode.text = value;
        }
    }
}