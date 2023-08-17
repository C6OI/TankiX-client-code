using UnityEngine;

namespace Lobby.ClientControls.API {
    public class InputFieldSubmitSoundEffectController : AbstractInputFieldSoundEffectController {
        public override string HandlerName => "Submit";

        void OnGUI() {
            if (Selected &&
                Event.current.type == EventType.KeyDown &&
                (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
                PlaySoundEffect();
            }
        }
    }
}