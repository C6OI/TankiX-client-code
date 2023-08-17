using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [SerialVersionUID(1436343541876L)]
    [Shared]
    public class ProficiencyLevelItemComponent : Component {
        public int Level { get; set; }
    }
}