using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [SerialVersionUID(1462285757477L)]
    [Shared]
    public class PersonalBattleInfoEvent : Event {
        public PersonalBattleInfo Info { get; set; }
    }
}