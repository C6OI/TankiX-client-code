using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1436339031431L)]
    [Shared]
    public class UpgradeLevelItemComponent : Component {
        public long Level { get; set; }
    }
}