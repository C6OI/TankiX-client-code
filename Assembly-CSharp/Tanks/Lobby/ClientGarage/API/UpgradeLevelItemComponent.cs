using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1436343541876L)]
    public class UpgradeLevelItemComponent : Component {
        public int Level { get; set; }
    }
}