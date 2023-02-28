using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    [Shared]
    [SerialVersionUID(1436338996992L)]
    public class ExperienceItemComponent : Component {
        public long Experience { get; set; }
    }
}