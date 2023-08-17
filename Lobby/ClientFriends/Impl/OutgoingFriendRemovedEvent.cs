using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.Impl {
    [SerialVersionUID(1450343225471L)]
    [Shared]
    public class OutgoingFriendRemovedEvent : FriendRemovedBaseEvent { }
}