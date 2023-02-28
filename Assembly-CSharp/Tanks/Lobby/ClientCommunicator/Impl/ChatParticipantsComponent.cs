using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(636437655901996504L)]
    public class ChatParticipantsComponent : Component {
        public List<Entity> Users { get; set; }
    }
}