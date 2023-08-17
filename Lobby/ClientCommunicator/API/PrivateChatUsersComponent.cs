using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.API {
    [Shared]
    [SerialVersionUID(1447314567734L)]
    public class PrivateChatUsersComponent : Component {
        public HashSet<Entity> Users { get; set; }
    }
}