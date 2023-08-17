using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Tanks.Lobby.ClientPaymentGUI.Impl {
    public class ForceEnterEmailScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text confirmButton;

        [SerializeField] Text rightPanelHint;

        [SerializeField] Text logoutButton;

        [SerializeField] Text logoutButtonYes;

        [SerializeField] Text logoutButtonNo;

        public string ConfirmButton {
            set => confirmButton.text = value;
        }

        public string RightPanelHint {
            set => rightPanelHint.text = value;
        }

        public string LogoutButton {
            set => logoutButton.text = value;
        }

        public string LogoutButtonYes {
            set => logoutButtonYes.text = value;
        }

        public string LogoutButtonNo {
            set => logoutButtonNo.text = value;
        }
    }
}