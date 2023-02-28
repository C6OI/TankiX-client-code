using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class UnLoadUsersComponent : Component {
        public UnLoadUsersComponent(HashSet<long> userIds) => UserIds = userIds;

        public HashSet<long> UserIds { get; }
    }
}