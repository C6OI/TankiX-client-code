using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    [Shared]
    [SerialVersionUID(1498740539984L)]
    public class LoadSortedFriendsIdsWithNicknamesEvent : Event { }
}