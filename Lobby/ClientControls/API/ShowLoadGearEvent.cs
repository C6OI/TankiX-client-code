using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientControls.API {
    public class ShowLoadGearEvent : Event {
        public ShowLoadGearEvent() => ShowProgress = false;

        public ShowLoadGearEvent(bool showProgress) => ShowProgress = showProgress;

        public bool ShowProgress { get; set; }
    }
}