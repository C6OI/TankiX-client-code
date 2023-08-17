using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1436434151462L)]
    public class ItemProficiencyUpdatedEvent : Event {
        public int Level { get; set; }
    }
}