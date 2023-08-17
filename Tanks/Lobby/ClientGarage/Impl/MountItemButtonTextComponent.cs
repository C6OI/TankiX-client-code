using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MountItemButtonTextComponent : Component {
        public string MountText { get; set; }

        public string MountedText { get; set; }
    }
}