using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1438169799324L)]
    [Shared]
    public class SupplyCountChangedEvent : Event { }
}