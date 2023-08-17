using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientEntrance.Impl {
    public class ChangeUserNameScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text inputHint;

        [SerializeField] Text continueButton;

        [SerializeField] Text reservedNameHint;

        public string InputHint {
            set => inputHint.text = value;
        }

        public string ContinueButton {
            set => continueButton.text = value;
        }

        public string ReservedNameHint {
            set => reservedNameHint.text = value;
        }
    }
}