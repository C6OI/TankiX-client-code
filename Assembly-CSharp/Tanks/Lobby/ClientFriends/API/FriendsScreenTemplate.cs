using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientFriends.Impl;
using Tanks.Lobby.ClientNavigation.API;

namespace Tanks.Lobby.ClientFriends.API {
    [SerialVersionUID(1450257440549L)]
    public interface FriendsScreenTemplate : ScreenTemplate, Template {
        [PersistentConfig]
        FriendsScreenLocalizationComponent friendsScreenLocalization();
    }
}