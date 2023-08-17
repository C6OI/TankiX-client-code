using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class VisibleItemsRangeComponent : Component {
        public VisibleItemsRangeComponent() { }

        public VisibleItemsRangeComponent(IndexRange range) => Range = range;

        public IndexRange Range { get; set; }
    }
}