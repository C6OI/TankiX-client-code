using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ExitGameDialog : ConfirmWindowComponent {
        protected override void OnEnable() {
            base.OnEnable();
            DailyBonusScreenSoundsRoot component = UISoundEffectController.UITransformRoot.GetComponent<DailyBonusScreenSoundsRoot>();

            if ((bool)component) {
                component.dailyBonusSoundsBehaviour.PlayClick();
            }
        }
    }
}