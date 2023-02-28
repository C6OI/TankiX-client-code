using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1496750075382L)]
    public class CreateCustomBattleLobbyEvent : Event {
        public ClientBattleParams Params { get; set; }
    }
}