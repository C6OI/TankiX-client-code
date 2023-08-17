using Lobby.ClientFriends.Impl;
using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.API {
    [SerialVersionUID(1450257440549L)]
    public interface FriendsScreenTemplate : ScreenTemplate, Template {
        [PersistentConfig]
        FriendsScreenLocalizationComponent friendsScreenLocalization();
    }
}