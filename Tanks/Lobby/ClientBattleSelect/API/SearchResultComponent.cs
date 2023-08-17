using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    public class SearchResultComponent : Component {
        public List<BattleEntry> PinnedBattles { get; set; } = new();

        public List<PersonalBattleInfo> PersonalInfos { get; set; } = new();

        public int BattlesCount { get; set; }
    }
}