using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientControls.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1441700384526L)]
    public class SearchRequestChangedEvent : Event {
        public SearchRequestChangedEvent() { }

        public SearchRequestChangedEvent(IndexRange range) => Range = range;

        public IndexRange Range { get; set; }
    }
}