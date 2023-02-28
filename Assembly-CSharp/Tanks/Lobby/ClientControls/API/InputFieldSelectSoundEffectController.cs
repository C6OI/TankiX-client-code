using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientControls.API {
    public class InputFieldSelectSoundEffectController : AbstractInputFieldSoundEffectController, IPointerDownHandler, IEventSystemHandler {
        public override string HandlerName => "Select";

        public void OnPointerDown(PointerEventData eventData) {
            if (!Selected) {
                PlaySoundEffect();
            }
        }
    }
}