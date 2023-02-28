using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    public class SearchDataComponent : Component {
        public SearchDataComponent(BattleEntry battleEntry, int indexInSearchResult) {
            BattleEntry = battleEntry;
            IndexInSearchResult = indexInSearchResult;
        }

        public BattleEntry BattleEntry { get; set; }

        public int IndexInSearchResult { get; set; }
    }
}