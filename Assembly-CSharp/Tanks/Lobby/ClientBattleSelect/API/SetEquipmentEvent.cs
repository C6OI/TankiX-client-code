using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientBattleSelect.API {
    [Shared]
    [SerialVersionUID(1496905821016L)]
    public class SetEquipmentEvent : Event {
        public long WeaponId { get; set; }

        public long HullId { get; set; }
    }
}