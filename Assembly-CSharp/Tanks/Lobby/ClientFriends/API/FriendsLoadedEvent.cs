using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1451120695251L)]
    public class FriendsLoadedEvent : Event {
        public HashSet<long> AcceptedFriendsIds { get; set; }

        public HashSet<long> IncommingFriendsIds { get; set; }

        public HashSet<long> OutgoingFriendsIds { get; set; }
    }
}