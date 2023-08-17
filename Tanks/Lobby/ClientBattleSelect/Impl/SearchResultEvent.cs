using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(1436353183002L)]
    [Shared]
    public class SearchResultEvent : Event {
        public List<BattleEntry> NewBattleEntries { get; set; }

        public List<PersonalBattleInfo> NewPersonalBattleInfos { get; set; }

        public int BattlesCount { get; set; }
    }
}