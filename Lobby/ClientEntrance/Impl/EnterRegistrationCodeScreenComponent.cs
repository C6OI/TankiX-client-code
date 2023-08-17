using Lobby.ClientControls.API;
using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Lobby.ClientEntrance.Impl {
    public class EnterRegistrationCodeScreenComponent : BehaviourComponent {
        [SerializeField] GameObject changeNameLink;

        [SerializeField] GameObject reservationCodeHint;

        [SerializeField] GameObject reservedNameHint;

        [SerializeField] GameObject backButton;

        [SerializeField] InputFieldComponent registrationCodeField;

        public bool ChangeNameLinkActivity {
            get => changeNameLink.activeSelf;
            set => changeNameLink.SetActive(value);
        }

        public bool ReservationCodeHintActivity {
            get => reservationCodeHint.activeSelf;
            set => reservationCodeHint.SetActive(value);
        }

        public bool ReservedNameHintActivity {
            get => reservedNameHint.activeSelf;
            set => reservedNameHint.SetActive(value);
        }

        public bool BackButtonActivity {
            get => backButton.activeSelf;
            set => backButton.SetActive(value);
        }

        public string InputtedRegistrationCode => registrationCodeField.Input;
    }
}