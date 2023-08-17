using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientControls.API {
    public class ItemsVisibilityChangedEvent : Event {
        public ItemsVisibilityChangedEvent(IndexRange prevRange, IndexRange range) {
            PrevRange = prevRange;
            Range = range;
        }

        public IndexRange PrevRange { get; set; }

        public IndexRange Range { get; set; }
    }
}