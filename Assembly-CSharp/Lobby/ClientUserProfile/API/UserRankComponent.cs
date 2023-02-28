using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(-1413405458500615976L)]
    public class UserRankComponent : Component {
        public int Rank { get; set; }
    }
}