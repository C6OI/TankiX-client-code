using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientEntrance.Impl {
    public class EnterRegistrationCodeScreenTextComponent : LocalizedScreenComponent {
        [SerializeField] Text haveNotRegistrationCode;

        [SerializeField] Text inputHint;

        [SerializeField] Text continueButton;

        [SerializeField] Text reservationCodeHint;

        [SerializeField] Text reservedNameHint;

        public string HaveNotRegistrationCode {
            set => haveNotRegistrationCode.text = value;
        }

        public string InputHint {
            set => inputHint.text = value;
        }

        public string ContinueButton {
            set => continueButton.text = value;
        }

        public string ReservationCodeHint {
            set => reservationCodeHint.text = value;
        }

        public string ReservedNameHint {
            set => reservedNameHint.text = value;
        }

        public string Error { get; set; }
    }
}