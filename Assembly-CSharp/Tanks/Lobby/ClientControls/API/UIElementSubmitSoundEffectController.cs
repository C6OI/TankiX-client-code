using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientControls.API {
    public class UIElementSubmitSoundEffectController : UISoundEffectController, IPointerClickHandler, ISubmitHandler, IEventSystemHandler {
        const string HANDLER_NAME = "ClickAndSubmit";

        public override string HandlerName => "ClickAndSubmit";

        public void OnPointerClick(PointerEventData eventData) {
            if (gameObject.IsInteractable()) {
                PlaySoundEffect();
            }
        }

        public void OnSubmit(BaseEventData eventData) {
            PlaySoundEffect();
        }
    }
}