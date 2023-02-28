using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1475648914994L)]
    public class CompleteBuyUIDChangeEvent : Event {
        public bool Success { get; set; }
    }
}