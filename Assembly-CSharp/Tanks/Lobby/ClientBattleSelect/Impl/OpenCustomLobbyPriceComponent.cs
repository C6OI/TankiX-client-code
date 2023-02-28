using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1548677305789L)]
    public class OpenCustomLobbyPriceComponent : Component {
        public long OpenPrice { get; set; }
    }
}