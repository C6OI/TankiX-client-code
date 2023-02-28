using Platform.Library.ClientUnityIntegration.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class PhoneInputComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI code;

        public void SetCode(string code) {
            this.code.text = code;
        }
    }
}