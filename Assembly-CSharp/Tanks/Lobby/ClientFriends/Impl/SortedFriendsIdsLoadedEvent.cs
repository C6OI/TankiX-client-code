using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    [Shared]
    [SerialVersionUID(1457951948522L)]
    public class SortedFriendsIdsLoadedEvent : Event {
        public Dictionary<long, string> friendsAcceptedIds { get; set; }

        public Dictionary<long, string> friendsIncomingIds { get; set; }

        public Dictionary<long, string> friendsOutgoingIds { get; set; }
    }
}