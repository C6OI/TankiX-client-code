using UnityEngine.EventSystems;

namespace Tanks.Lobby.ClientControls.API {
    public abstract class AbstractInputFieldSoundEffectController : UISoundEffectController, ISelectHandler, IDeselectHandler, IEventSystemHandler {
        protected bool Selected { get; private set; }

        void OnEnable() {
            Selected = false;
        }

        public void OnDeselect(BaseEventData eventData) {
            Selected = false;
        }

        public void OnSelect(BaseEventData eventData) {
            Selected = true;
        }
    }
}