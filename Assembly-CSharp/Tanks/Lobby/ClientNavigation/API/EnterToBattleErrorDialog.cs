using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.API {
    public class EnterToBattleErrorDialog : NotificationDialogComponent {
        [SerializeField] LocalizedField errorText;

        public void Show() {
            base.Show(errorText.Value);
        }
    }
}