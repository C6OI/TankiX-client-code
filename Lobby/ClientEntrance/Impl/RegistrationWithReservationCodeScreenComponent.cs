using Lobby.ClientNavigation.API;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.ClientEntrance.Impl {
    public class RegistrationWithReservationCodeScreenComponent : LocalizedScreenComponent {
        [SerializeField] Text haveRegistrationCode;

        public string HaveRegistrationCode {
            set => haveRegistrationCode.text = value;
        }
    }
}