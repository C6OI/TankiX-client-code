using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientEntrance.Impl {
    public class InviteScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] Text inputHint;

        [SerializeField] Text continueButton;

        public string InputHint {
            set => inputHint.text = value;
        }

        public string Continue {
            set => continueButton.text = value;
        }

        public string Error { get; set; }
    }
}