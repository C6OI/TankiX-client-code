using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(1446726412859L)]
    public class ChatActiveUserListComponent : Component {
        public List<Entity> Users { get; set; }
    }
}