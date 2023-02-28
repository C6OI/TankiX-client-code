using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class LoadUsersComponent : Component {
        public LoadUsersComponent(HashSet<long> userIds) => UserIds = userIds;

        public HashSet<long> UserIds { get; }
    }
}