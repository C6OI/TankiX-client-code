using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(-1413405458500615976L)]
    [Shared]
    public class UserRankComponent : Component {
        public int Rank { get; set; }
    }
}