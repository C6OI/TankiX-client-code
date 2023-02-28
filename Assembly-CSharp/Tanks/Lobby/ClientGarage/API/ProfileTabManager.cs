using Tanks.Lobby.ClientControls.API;
using Tanks.Lobby.ClientGarage.Impl;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ProfileTabManager : TabManager {
        protected override void OnEnable() {
            Show(index);
        }

        protected override void OnDisable() {
            base.OnDisable();
            index = 0;
        }

        public override void Show(int newIndex) {
            base.Show(newIndex);
            LogScreen screen = newIndex != 1 ? LogScreen.ProfileSummary : LogScreen.ProfileAccount;
            MainScreenComponent.Instance.SendShowScreenStat(screen);
        }
    }
}