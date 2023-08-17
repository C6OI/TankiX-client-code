using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [SerialVersionUID(1458555309592L)]
    [Shared]
    public class UnLoadUsersEvent : Event {
        public UnLoadUsersEvent(HashSet<long> userIds) => UserIds = userIds;

        public UnLoadUsersEvent(params long[] userIds) => UserIds = new HashSet<long>(userIds);

        public HashSet<long> UserIds { get; set; }
    }
}