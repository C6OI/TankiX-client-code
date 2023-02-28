using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;
using Tanks.Lobby.ClientFriends.Impl;

namespace Tanks.Lobby.ClientFriends.API {
    [Shared]
    [SerialVersionUID(1450168255217L)]
    public class AcceptFriendEvent : FriendBaseEvent {
        public AcceptFriendEvent() { }

        public AcceptFriendEvent(Entity user)
            : base(user) { }
    }
}