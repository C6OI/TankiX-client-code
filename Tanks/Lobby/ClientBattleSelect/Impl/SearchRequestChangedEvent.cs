using Lobby.ClientControls.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(1441700384526L)]
    [Shared]
    public class SearchRequestChangedEvent : Event {
        public SearchRequestChangedEvent() { }

        public SearchRequestChangedEvent(IndexRange range) => Range = range;

        public IndexRange Range { get; set; }
    }
}