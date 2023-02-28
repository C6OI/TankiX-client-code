using Platform.Library.ClientUnityIntegration.API;
using Tanks.Lobby.ClientControls.API;
using TMPro;

namespace Tanks.Battle.ClientHUD.Impl {
    public class StreakTerminationUIComponent : BehaviourComponent {
        public TextMeshProUGUI streakTerminationText;

        public LocalizedField streakTerminationLocalization;
    }
}