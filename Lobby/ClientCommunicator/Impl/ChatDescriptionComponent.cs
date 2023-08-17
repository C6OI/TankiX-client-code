using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(1451370440350L)]
    public class ChatDescriptionComponent : Component {
        public Optional<long> ChatEntityId { get; set; }
    }
}