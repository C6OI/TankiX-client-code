using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(1479374709878L)]
    public class NewsItemComponent : Component {
        public NewsItem Data { get; set; }

        [ProtocolTransient] public int ShowCount { get; set; }
    }
}