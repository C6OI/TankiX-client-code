using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.Impl {
    [Shared]
    [SerialVersionUID(1457951948522L)]
    public class SortedFriendsIdsLoadedEvent : Event {
        public List<long> FriendsIds { get; set; }
    }
}