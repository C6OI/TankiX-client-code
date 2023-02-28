using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.Impl {
    [Shared]
    [SerialVersionUID(1510173920293L)]
    public class ShareEnergyEvent : Event {
        public long ReceiverId { get; set; }
    }
}