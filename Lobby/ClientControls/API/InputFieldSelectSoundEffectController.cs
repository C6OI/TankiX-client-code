using UnityEngine.EventSystems;

namespace Lobby.ClientControls.API {
    public class InputFieldSelectSoundEffectController : AbstractInputFieldSoundEffectController, IEventSystemHandler,
        IPointerDownHandler {
        public override string HandlerName => "Select";

        public void OnPointerDown(PointerEventData eventData) {
            if (!Selected) {
                PlaySoundEffect();
            }
        }
    }
}