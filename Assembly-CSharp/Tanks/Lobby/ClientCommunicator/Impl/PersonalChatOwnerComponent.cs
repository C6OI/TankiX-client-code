using System.Collections.Generic;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(1513067769958L)]
    public class PersonalChatOwnerComponent : Component {
        public List<long> ChatsIs { get; set; }
    }
}