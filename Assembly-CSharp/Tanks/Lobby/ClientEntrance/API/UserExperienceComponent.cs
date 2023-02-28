using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.API {
    [Shared]
    [SerialVersionUID(-777019732837383198L)]
    public class UserExperienceComponent : Component {
        public long Experience { get; set; }
    }
}