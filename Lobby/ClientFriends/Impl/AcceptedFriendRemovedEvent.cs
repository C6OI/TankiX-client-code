using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.Impl {
    [SerialVersionUID(1450343296915L)]
    [Shared]
    public class AcceptedFriendRemovedEvent : FriendRemovedBaseEvent { }
}