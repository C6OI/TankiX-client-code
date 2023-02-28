using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636475130636226702L)]
    public class PendingInvitesComponent : Component {
        public List<Entity> Users { get; set; }
    }
}