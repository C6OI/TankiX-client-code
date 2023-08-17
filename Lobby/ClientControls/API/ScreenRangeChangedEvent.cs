using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientControls.API {
    public class ScreenRangeChangedEvent : Event {
        public ScreenRangeChangedEvent(IndexRange range) => Range = range;

        public IndexRange Range { get; set; }
    }
}