using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(635902862624765629L)]
    public class UnconfirmedUserEmailComponent : Component {
        public string Email { get; set; }
    }
}