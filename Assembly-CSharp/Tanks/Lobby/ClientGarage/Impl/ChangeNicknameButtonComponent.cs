using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ChangeNicknameButtonComponent : BehaviourComponent {
        [SerializeField] TextMeshProUGUI price;

        [SerializeField] PaletteColorField notEnoughColor;

        public bool Enough {
            set {
                if (value) {
                    price.color = Color.white;
                } else {
                    price.color = notEnoughColor.Color;
                }
            }
        }

        public string XPrice {
            set => price.text = value + "<sprite=9>";
        }
    }
}