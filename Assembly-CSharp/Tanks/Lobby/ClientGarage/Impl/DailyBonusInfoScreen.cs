using Platform.Library.ClientUnityIntegration.API;
using UnityEngine;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DailyBonusInfoScreen : ConfirmDialogComponent {
        [SerializeField] DailyBonusScreenComponent dailyBonusScreen;

        void Update() {
            if (InputMapping.Cancel) {
                Hide();
            }
        }

        public void Show() {
            dailyBonusScreen.Hide();
            MainScreenComponent.Instance.OverrideOnBack(Hide);
            Show(null);
        }

        public override void Hide() {
            base.Hide();
            MainScreenComponent.Instance.ClearOnBackOverride();
            dailyBonusScreen.Show();
        }
    }
}