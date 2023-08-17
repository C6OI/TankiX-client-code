using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.Impl {
    [SerialVersionUID(1450343151033L)]
    [Shared]
    public class IncomingFriendRemovedEvent : FriendRemovedBaseEvent { }
}