using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1544499423535L)]
    public class FractionComponent : Component {
        public string FractionName { get; set; }
    }
}