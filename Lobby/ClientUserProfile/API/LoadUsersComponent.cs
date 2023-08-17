using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.API {
    public class LoadUsersComponent : Component {
        public LoadUsersComponent(HashSet<long> userIds) => UserIds = userIds;

        public HashSet<long> UserIds { get; private set; }
    }
}