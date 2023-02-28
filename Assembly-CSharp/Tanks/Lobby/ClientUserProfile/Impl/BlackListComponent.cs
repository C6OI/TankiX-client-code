using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [Shared]
    [SerialVersionUID(1507197930106L)]
    public class BlackListComponent : Component {
        public List<long> BlockedUsers { get; set; }
    }
}