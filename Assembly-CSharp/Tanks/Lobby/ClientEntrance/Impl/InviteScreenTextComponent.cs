using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientEntrance.Impl {
    public class InviteScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI inputHint;

        [SerializeField] TextMeshProUGUI continueButton;

        public string InputHint {
            set => inputHint.text = value;
        }

        public string Continue {
            set => continueButton.text = value;
        }

        public string Error { get; set; }
    }
}