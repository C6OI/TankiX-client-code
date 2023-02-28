using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientNavigation.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class FullEnergyDialog : NotificationDialogComponent {
        [SerializeField] LocalizedField errorText;

        public void Show() {
            base.Show(errorText.Value);
        }
    }
}