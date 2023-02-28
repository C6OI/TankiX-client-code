using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientMatchMaking.API {
    [SerialVersionUID(1493208024600L)]
    public interface MatchMakingTemplate : Template {
        [AutoAdded]
        MatchMakingComponent matchMaking();
    }
}