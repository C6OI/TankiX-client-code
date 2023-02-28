using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class VisibleItemsRangeComponent : Component {
        public VisibleItemsRangeComponent() { }

        public VisibleItemsRangeComponent(IndexRange range) => Range = range;

        public IndexRange Range { get; set; }
    }
}