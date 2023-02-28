using System.Collections.Generic;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class UsersLoadedComponent : LoadUsersComponent {
        public UsersLoadedComponent(HashSet<long> userIds)
            : base(userIds) { }
    }
}