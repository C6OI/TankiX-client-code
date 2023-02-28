using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    [SerialVersionUID(1481257271234L)]
    public interface ConfiguredNewsItemTemplate : Template {
        NewsItemComponent newsItem();
    }
}