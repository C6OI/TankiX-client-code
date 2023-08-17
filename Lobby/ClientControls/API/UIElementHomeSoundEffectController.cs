using UnityEngine;

namespace Lobby.ClientControls.API {
    public class UIElementHomeSoundEffectController : UISoundEffectController {
        public override string HandlerName => "Home";

        void OnDisable() {
            if (alive) {
                PlayHomeSoundEffectIfNeeded();
            }
        }

        void PlayHomeSoundEffectIfNeeded() {
            if (Input.GetKey(KeyCode.Home)) {
                PlaySoundEffect();
            }
        }
    }
}