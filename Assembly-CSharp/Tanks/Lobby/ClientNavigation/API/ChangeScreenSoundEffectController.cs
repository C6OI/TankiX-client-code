using Tanks.Lobby.ClientControls.API;
using UnityEngine;

namespace Tanks.Lobby.ClientNavigation.API {
    [RequireComponent(typeof(Animator))]
    public class ChangeScreenSoundEffectController : UISoundEffectController {
        const string HANDLER_NAME = "ChangeScreen";

        public override string HandlerName => "ChangeScreen";

        void OnChangeScreen() {
            PlaySoundEffect();
        }
    }
}