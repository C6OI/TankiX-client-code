using UnityEngine.EventSystems;

namespace Lobby.ClientControls.API {
    public abstract class AbstractInputFieldSoundEffectController : UISoundEffectController, IDeselectHandler,
        IEventSystemHandler, ISelectHandler {
        protected bool Selected { get; private set; }

        void OnEnable() => Selected = false;

        public void OnDeselect(BaseEventData eventData) => Selected = false;

        public void OnSelect(BaseEventData eventData) => Selected = true;
    }
}