using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientFriends.Impl {
    [Shared]
    [SerialVersionUID(1450343225471L)]
    public class OutgoingFriendRemovedEvent : FriendRemovedBaseEvent { }
}