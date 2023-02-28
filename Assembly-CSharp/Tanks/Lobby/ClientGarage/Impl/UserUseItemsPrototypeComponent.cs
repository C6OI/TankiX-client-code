using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(5667343880942321237L)]
    public class UserUseItemsPrototypeComponent : SharedChangeableComponent {
        public Entity PrototypeUser { get; set; }

        public Entity Preset { get; set; }
    }
}