using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientControls.API {
    public class ScreenRangeChangedEvent : Event {
        public ScreenRangeChangedEvent(IndexRange range) => Range = range;

        public IndexRange Range { get; set; }
    }
}