using Lobby.ClientFriends.Impl;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1450168139800L)]
    public class RequestFriendEvent : FriendBaseEvent {
        public RequestFriendEvent() { }

        public RequestFriendEvent(Entity user)
            : base(user) { }
    }
}