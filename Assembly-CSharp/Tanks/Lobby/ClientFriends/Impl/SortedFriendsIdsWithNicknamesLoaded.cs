using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    [Shared]
    [SerialVersionUID(1498741007777L)]
    public class SortedFriendsIdsWithNicknamesLoaded : Event {
        public Dictionary<long, string> FriendsIdsAndNicknames { get; set; }
    }
}