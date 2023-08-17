using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1475648914994L)]
    [Shared]
    public class CompleteBuyUIDChangeEvent : Event {
        public bool Success { get; set; }
    }
}