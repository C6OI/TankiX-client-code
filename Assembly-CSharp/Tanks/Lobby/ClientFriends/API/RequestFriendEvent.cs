using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientFriends.Impl;

namespace Tanks.Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1450168139800L)]
    public class RequestFriendEvent : FriendBaseEvent {
        public RequestFriendEvent() { }

        public RequestFriendEvent(Entity user)
            : base(user) { }
    }
}