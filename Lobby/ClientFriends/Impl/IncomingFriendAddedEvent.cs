using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.Impl {
    [Shared]
    [SerialVersionUID(1450343100021L)]
    public class IncomingFriendAddedEvent : FriendAddedBaseEvent { }
}