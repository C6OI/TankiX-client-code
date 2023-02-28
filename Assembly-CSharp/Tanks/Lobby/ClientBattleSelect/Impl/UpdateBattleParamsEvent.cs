using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1497614958932L)]
    public class UpdateBattleParamsEvent : Event {
        public ClientBattleParams Params { get; set; }
    }
}