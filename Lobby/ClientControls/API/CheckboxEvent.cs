using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientControls.API {
    public class CheckboxEvent : Event {
        protected bool isChecked;

        public bool IsChecked => isChecked;
    }
}