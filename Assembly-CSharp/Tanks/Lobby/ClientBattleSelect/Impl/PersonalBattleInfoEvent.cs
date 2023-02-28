using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientBattleSelect.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1462285757477L)]
    public class PersonalBattleInfoEvent : Event {
        public PersonalBattleInfo Info { get; set; }
    }
}