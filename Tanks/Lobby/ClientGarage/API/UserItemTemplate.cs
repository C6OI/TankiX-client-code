using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [SerialVersionUID(635719627719140726L)]
    public interface UserItemTemplate : Template {
        [AutoAdded]
        UserItemComponent userItem();
    }
}