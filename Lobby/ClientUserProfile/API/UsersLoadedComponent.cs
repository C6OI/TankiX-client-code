using System.Collections.Generic;

namespace Lobby.ClientUserProfile.API {
    public class UsersLoadedComponent : LoadUsersComponent {
        public UsersLoadedComponent(HashSet<long> userIds)
            : base(userIds) { }
    }
}