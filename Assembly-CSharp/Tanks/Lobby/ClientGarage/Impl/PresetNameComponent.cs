using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1493974995307L)]
    public class PresetNameComponent : SharedChangeableComponent {
        public string Name { get; set; }
    }
}