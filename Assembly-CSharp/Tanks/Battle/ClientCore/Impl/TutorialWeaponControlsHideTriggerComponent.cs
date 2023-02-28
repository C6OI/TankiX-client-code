using Platform.Kernel.OSGi.ClientCore.API;
using Tanks.Lobby.ClientGarage.Impl;

namespace Tanks.Battle.ClientCore.Impl {
    public class TutorialWeaponControlsHideTriggerComponent : TutorialHideTriggerComponent {
        [Inject] public static InputManager InputManager { get; set; }

        protected void Update() {
            if (!triggered && InputManager.CheckAction(ShotActions.SHOT)) {
                Triggered();
            }
        }
    }
}