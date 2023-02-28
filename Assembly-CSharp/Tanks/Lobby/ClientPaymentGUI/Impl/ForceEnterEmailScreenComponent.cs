using Tanks.Lobby.ClientNavigation.API;
using TMPro;
using UnityEngine;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ForceEnterEmailScreenComponent : LocalizedScreenComponent {
        [SerializeField] TextMeshProUGUI confirmButton;

        [SerializeField] TextMeshProUGUI rightPanelHint;

        [SerializeField] TextMeshProUGUI placeholder;

        public string ConfirmButton {
            set => confirmButton.text = value;
        }

        public string RightPanelHint {
            set => rightPanelHint.text = value;
        }

        public string Placeholder {
            set => placeholder.text = value;
        }
    }
}