using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientControls.API {
    public class ConfirmButtonClickEvent : Event {
        public long MissingCrystalsAmount { get; set; }

        public long MissingXCrystalsAmount { get; set; }
    }
}